using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] string _spelltype = "";
    [SerializeField] RuntimeData _runtimeData;


    private void Awake()
    {
        
    }

    private void Start()
    {
        if (_spelltype == "light")
        {
            Invoke("selfDestruct", 10);
            GameEvents.InvokePlayAudio("light");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            if (_spelltype == "snare") performSnare(other);
            if (_spelltype == "teleportIn") performTP(other);
        }
        if (other.CompareTag("Player"))
        {
            if (_spelltype == "teleportIn") performTP(other);
        }
    }

    private void performSnare(Collider other)
    {
        other.transform.GetComponent<Enemy>().snare(6);
        transform.GetComponent<ParticleSystem>().Play(true); // play particle system and children particle systems
        transform.Find("light").GetComponent<Light>().enabled = true;
        GameEvents.InvokePlayAudio("snare");
        Invoke("stopSnare", 6);
    }

    private void stopSnare()
    {
        transform.GetComponent<ParticleSystem>().Stop(true);
        transform.Find("light").GetComponent<Light>().enabled = false;
    }

    private void performTP(Collider other)
    {
        if (_runtimeData.teleportOut != null)
        {
            GameEvents.InvokePlayAudio("teleport");
            // teleport in particle effects
            transform.GetComponent<ParticleSystem>().Play(true); // play particle system and children particle systems

            // teleport out particle effects
            _runtimeData.teleportOut.GetComponent<ParticleSystem>().Play(true);
            if(other.CompareTag("Player")) other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = _runtimeData.teleportOut.position;
            if (other.CompareTag("Player")) other.GetComponent<CharacterController>().enabled = true;
        }
    }

    private void selfDestruct()
    {
        Destroy(gameObject);
    }
}
