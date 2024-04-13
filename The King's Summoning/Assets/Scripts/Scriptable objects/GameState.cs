using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateGameStateSO", order = 1)]
public class GameState : ScriptableSingleton<GameState>
{
    public enum Level{
        Start,
        PerfectCircle,
        AppleRiver,
        KingsArmor,
    }

    [SerializeField] public Level currentLevel;
}
