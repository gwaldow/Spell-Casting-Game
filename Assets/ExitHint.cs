using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) GameEvents.InvokeDisplayMessage("A grate to the outside! If only there were some way to teleport through it!", 5);
    }
}
