using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSystem", menuName = "ScriptableObjects/CreateDialogueSystem", order = 2)]
public class DialogueSystem : ScriptableSingleton<DialogueSystem>
{
    public enum Character{
        TheFatKing,
        Squire
    }

    public int CurrentChapter;
    public int CurrentDialogue;
    
    public Chapter[] Chapters;
}
