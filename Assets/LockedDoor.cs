using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] RuntimeData _runtimeData;
    [SerializeField] string _doorName;
    private void OnMouseDown()
    {
        if(_doorName == "front")
        {
            GameEvents.InvokePlayAudio("lockeddoor");
            GameEvents.InvokeDisplayMessage("It seems to be shut... permenantly", 2);
            return;
        }
        if (_runtimeData.hasKey)
        {
            GameEvents.InvokePlayAudio("dooropen");
            Destroy(gameObject);
        }
        else
        {
            GameEvents.InvokePlayAudio("lockeddoor");
            GameEvents.InvokeDisplayMessage("It's Locked.", 2);
        }
    }
}
