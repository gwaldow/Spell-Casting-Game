using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [SerializeField] float _mouseSensitivity = 6f;
    [SerializeField] float _currentTilt = 0f;
    [SerializeField] float _walkSpeed = 3f;
    [SerializeField] float _runSpeed = 4f;
    [SerializeField] float _jumpPower = 4f;
    [SerializeField] float _gravity = 1f;
    [SerializeField] float _chalk = 0f;
    [SerializeField] float _castRange = 3f;
    [SerializeField] LayerMask _canBeCast;
    [SerializeField] float _maxStamina = 5;
    [SerializeField] float _health = 100;
    [SerializeField] GameObject _castingFramePrefab;
    [SerializeField] Camera _cam;
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] GameObject _enemy;
    private CharacterController controller;
    private GameObject ghostCastingFrame = null;
    private GameObject placedCastingFrame = null;
    private float _stamina = 5;
    private bool _canBook = false;
    private bool _firstChalk = true;
    private AudioSource myAudio;
    // Start is called before the first frame update
    // github hello?
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        myAudio = GetComponent<AudioSource>();
        GameEvents.AquireBook += AquireBook;
        GameEvents.AquireChalk += AquireChalk;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_runtimeData.CurrentMenuState)
        {
            case Enums.MenuState.Free:
                {
                    Aim();
                    Movement();
                    Rune();
                    Book();
                    UI();
                    Die();
                    break;
                }
            case Enums.MenuState.Main:
                {

                    break;
                }
            case Enums.MenuState.Pause:
                {

                    break;
                }
            case Enums.MenuState.Casting:
                {
                    Book();
                    ExitCasting();
                    UI();
                    Die();
                    break;
                }
        }
    }

    void Aim()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // camera tilt computation
        _currentTilt -= mouseY * _mouseSensitivity;
        _currentTilt = Mathf.Clamp(_currentTilt, -90, 90);

        transform.Rotate(new Vector3(0, 1, 0), mouseX * _mouseSensitivity);
        _cam.transform.localEulerAngles = new Vector3(_currentTilt, 0f, 0f);
        //_cam.transform.Rotate(Vector3.right, mouseY * _mouseSensitivity);
    }

    void Movement()
    {
        // HORIZONTAL
        Vector3 sidewaysMovementVector = transform.right * Input.GetAxis("Horizontal"); // transform.right get the component of the vector with local space
        Vector3 forwardMovementVector = transform.forward * Input.GetAxis("Vertical");
        Vector3 movementVector = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftShift) && _stamina > 0)
        {
            movementVector = (sidewaysMovementVector + forwardMovementVector) * _runSpeed * Time.deltaTime;
            _stamina -= Time.deltaTime;
        }
        else
        {
            movementVector = (sidewaysMovementVector + forwardMovementVector) * _walkSpeed * Time.deltaTime;
            if(_stamina < _maxStamina) _stamina += Time.deltaTime;
        }

        if(controller.isGrounded && movementVector.magnitude > 0 && !myAudio.isPlaying)
        {
            myAudio.Play();
        }

        if(movementVector.magnitude == 0)
        {
            myAudio.Stop();
        }

        movementVector = movementVector + new Vector3(0, -_gravity * Time.deltaTime, 0);
        controller.Move(movementVector);
    }

    void Book()
    {
        if (Input.GetKey(KeyCode.Space) && _canBook)
        {
            transform.Find("Main Camera").transform.Find("PlayerTome").gameObject.SetActive(true);
            if (Input.GetKey(KeyCode.Q)) transform.Find("Main Camera").Find("PlayerTome").GetComponent<Book>().changePages(-2);
            if (Input.GetKey(KeyCode.E)) transform.Find("Main Camera").Find("PlayerTome").GetComponent<Book>().changePages(2);
        }
        else
        {
            transform.Find("Main Camera").transform.Find("PlayerTome").gameObject.SetActive(false);
        }
    }

    void UI()
    {
        GameEvents.InvokePlayerDamage();
    }
    void Rune()
    {
        if(Input.GetMouseButton(1))
        {
            if(_chalk >= 1)
            {

                // possible to cast, use raycast to aim
                RaycastHit aimHit = new RaycastHit();
                if(Physics.Raycast(_cam.transform.position, _cam.transform.forward, out aimHit, _castRange, _canBeCast))
                {
                    if (ghostCastingFrame == null)
                    {
                        ghostCastingFrame = Instantiate(_castingFramePrefab, aimHit.point + aimHit.normal * 0.001f, Quaternion.identity);
                    }
                    // adjust ghost frame position
                    ghostCastingFrame.transform.position = aimHit.point + aimHit.normal * 0.001f;
                    ghostCastingFrame.transform.LookAt(aimHit.point + aimHit.normal);
                    if (Input.GetMouseButtonDown(0))
                    {
                        GameEvents.InvokeDisplayMessage("Press \"ESC\" to exit rune forging", 2);
                        // begin rune casting
                        _chalk--; // use a chalk
                        // solidify a NEW "placed" casting frame.
                        Destroy(ghostCastingFrame);
                        placedCastingFrame = Instantiate(_castingFramePrefab, aimHit.point + aimHit.normal * 0.001f, Quaternion.identity);
                        placedCastingFrame.transform.LookAt(aimHit.point + aimHit.normal);
                        _cam.GetComponent<Camera>().enabled = false;
                        placedCastingFrame.transform.Find("Camera").GetComponent<Camera>().enabled = true;
                        placedCastingFrame.transform.Find("Camera").GetComponent<Draw>().enabled = true;
                        _runtimeData.CurrentMenuState = Enums.MenuState.Casting;
                        Cursor.lockState = CursorLockMode.Confined;
                    }
                }
                else
                {
                    destroyCastingFrame();
                }
            }
        }
        else
        {
            destroyCastingFrame();
        }
    }

    private void Die()
    {
        if (_runtimeData.playerHP <= 0) SceneManager.LoadScene(0);
    }

    private void ExitCasting()
    {
        // allow user to leave
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            placedCastingFrame.transform.Find("Camera").GetComponent<Draw>().RemoveSuperfluousDrawing();
            Destroy(placedCastingFrame);
            transform.Find("Main Camera").GetComponent<Camera>().enabled = true;
            _runtimeData.CurrentMenuState = Enums.MenuState.Free;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void destroyCastingFrame()
    {
        if (ghostCastingFrame != null)
        {
            Destroy(ghostCastingFrame);
            ghostCastingFrame = null;
        }
    }

    private void AquireBook()
    {
        _canBook = true;
    }

    private void AquireChalk()
    {
        if (_firstChalk)
        {
            GameEvents.InvokeDisplayMessage("Hold \"Right Click\" to propose a casting frame.\nClick \"Left Click\" to begin drawing a rune.", 6);
            _chalk++;
            _firstChalk = false;
        }
        else
        {
            GameEvents.InvokeDisplayMessage("+1 chalk", 1);
            _chalk++;
        }
    }

    public void damage(float dmg)
    {
        _health -= dmg;
        _runtimeData.playerHP = (int)_health;
    }
}