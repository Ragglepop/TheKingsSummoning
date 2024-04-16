using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/CreateGameState", order = 2)]
public class GameState : ScriptableObject
{
    public enum Character{
        TheFatKing,
        Squire
    }

    public int CurrentChapter;
    public int CurrentDialogue;
    
    public Chapter[] Chapters;
    public string LastLoadedLevel;
}
