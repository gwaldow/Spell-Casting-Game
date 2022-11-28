using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePickup : MonoBehaviour
{
    private Color mainColor;
    [SerializeField] List<Texture2D> pages;
    [SerializeField] string spellName;
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
        if (_runtimeData.hasBook)
        {
            GameEvents.InvokeAquirePages(pages);
            GameEvents.InvokePlayAudio("pagepickup");
            GameEvents.InvokeDisplayMessage(spellName + " rune added to tome", 3);
            Destroy(gameObject, 0.1f);
        }
    }
}
