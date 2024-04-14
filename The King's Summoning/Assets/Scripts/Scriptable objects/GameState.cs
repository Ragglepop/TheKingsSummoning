using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/CreateGameState", order = 2)]
public class GameState : ScriptableSingleton<GameState>
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
