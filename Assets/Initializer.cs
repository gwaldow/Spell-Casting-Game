using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] RuntimeData _runtimeData;
    void Awake()
    {
        _runtimeData.CurrentMenuState = Enums.MenuState.Free;
        _runtimeData.playerHP = 100;
        _runtimeData.hasKey = false;
        _runtimeData.hasBook = false;
    }

    void Start()
    {
        
    }
}