using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public GameObject King;
    public TMP_Text KingText;
    public GameObject Squire;
    public TMP_Text SquireText;

    // Start is called before the first frame update
    void Start()
    {
        DisplayDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next(){
        Chapter currChapter = GetCurrentChapter();
        Chapter.DialogueOption currOption = GetCurrentDialogue();
        GameState.instance.CurrentDialogue++;

        DisplayDialogue();
    }

    private Chapter GetCurrentChapter(){
        return GameState.instance.Chapters[GameState.instance.CurrentChapter];
    }

    private Chapter.DialogueOption GetCurrentDialogue(){
        Chapter currChapter = GetCurrentChapter();
        return currChapter.DialogueEntries[GameState.instance.CurrentDialogue];
    }

    private void DisplayDialogue(){
        Chapter.DialogueOption currDialogue = GetCurrentDialogue();
        if(currDialogue.character == GameState.Character.TheFatKing){
            Squire.SetActive(false);
            King.SetActive(true);
            KingText.text = currDialogue.Dialogue;
        }else if(currDialogue.character == GameState.Character.Squire){
            King.SetActive(false);
            Squire.SetActive(true);
            SquireText.text = currDialogue.Dialogue;
        }

        if(currDialogue.SceneToLoad!=String.Empty){
            GameState.instance.CurrentChapter++;
            GameState.instance.CurrentDialogue=0;
            LevelLoader.Load(currDialogue.SceneToLoad);
        }
    }
}
