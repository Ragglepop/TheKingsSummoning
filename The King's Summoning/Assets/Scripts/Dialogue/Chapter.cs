using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Chapter", menuName = "ScriptableObjects/CreateChapter", order = 3)]
public class Chapter : ScriptableObject
{
    [Serializable]
    public struct DialogueOption{
        public DialogueSystem.Character character;
        public string Dialogue;
        public string SceneToLoad;
    }
    
    public DialogueOption[] DialogueEntries;
}
