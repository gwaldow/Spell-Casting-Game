using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chalk : MonoBehaviour
{
    private Color mainColor;

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
        GameEvents.InvokeAquireChalk();
        GameEvents.InvokePlayAudio("pickup");
        Destroy(gameObject);
    }
}
