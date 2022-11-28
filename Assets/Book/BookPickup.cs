using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPickup : MonoBehaviour
{
    private Color mainColor;
    [SerializeField] RuntimeData _runtimeData;
    private void Start()
    {
        mainColor = transform.GetComponent<Renderer>().material.color;
    }
    private void OnMouseEnter()
    {
        // highlight or sumthin
        transform.GetComponent<Renderer>().material.color = Color.red;
    }

    private void OnMouseExit()
    {
        transform.GetComponent<Renderer>().material.color = mainColor;
    }

    private void OnMouseDown()
    {
        _runtimeData.hasBook = true;
        GameEvents.InvokeAquireBook();
        GameEvents.InvokePlayAudio("pagepickup");
        GameEvents.InvokeDisplayMessage("Hold \"Spacebar\" to read tome\nPress \"Q\" and \"E\" to flip pages", 5);
        Destroy(gameObject);
    }
}
