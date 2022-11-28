using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameEvents.InvokeDisplayMessage("YOU ESCAPED!!! Very Pog.", 4);
            Invoke("wingame", 5);
        }

    }
    private void wingame()
    {
        SceneManager.LoadScene(0);
    }
}
