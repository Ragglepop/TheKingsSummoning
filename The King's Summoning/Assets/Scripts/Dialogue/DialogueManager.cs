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
    public GameState state;
    public LevelLoader loader;

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
        state.CurrentDialogue++;

        DisplayDialogue();
    }

    private Chapter GetCurrentChapter(){
        return state.Chapters[state.CurrentChapter];
    }

    private Chapter.DialogueOption GetCurrentDialogue(){
        Chapter currChapter = GetCurrentChapter();
        return currChapter.DialogueEntries[state.CurrentDialogue];
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
            state.CurrentChapter++;
            state.CurrentDialogue=0;
            loader.LoadLevel(currDialogue.SceneToLoad);
        }
    }
}
