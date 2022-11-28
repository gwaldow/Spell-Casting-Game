using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "RuntimeData")]
public class RuntimeData : ScriptableObject
{
    public string currentItemHovered;
    public Enums.MenuState CurrentMenuState;
    public Transform teleportIn;
    public Transform teleportOut;
    public int playerHP;
    public bool hasKey;
    public bool hasBook;
}
