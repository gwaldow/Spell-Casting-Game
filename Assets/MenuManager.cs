using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI hptext;
    [SerializeField] TMPro.TextMeshProUGUI messagetext;
    [SerializeField] RuntimeData _runtimeData;
    void Start()
    {
        GameEvents.PlayerDamage += UpdatePlayerHP;
        GameEvents.DisplayMessage += DisplayMessage;
    }

    void UpdatePlayerHP()
    {
        hptext.text = "" + _runtimeData.playerHP;
    }

    void DisplayMessage(object sender, DisplayMessageArgs e)
    {
        messagetext.text = e.message;
        Invoke("RemoveMessage", e.displayTime);
    }

    void RemoveMessage()
    {
        messagetext.text = "";
    }
}
