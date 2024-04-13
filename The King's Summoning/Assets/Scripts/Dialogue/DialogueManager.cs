using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public DialogueSystem Dialogue;
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
        Dialogue.CurrentDialogue++;

        DisplayDialogue();
    }

    private Chapter GetCurrentChapter(){
        return Dialogue.Chapters[Dialogue.CurrentChapter];
    }

    private Chapter.DialogueOption GetCurrentDialogue(){
        Chapter currChapter = GetCurrentChapter();
        return currChapter.DialogueEntries[Dialogue.CurrentDialogue];
    }

    private void DisplayDialogue(){
        Chapter.DialogueOption currDialogue = GetCurrentDialogue();
        if(currDialogue.character == DialogueSystem.Character.TheFatKing){
            Squire.SetActive(false);
            King.SetActive(true);
            KingText.text = currDialogue.Dialogue;
        }else if(currDialogue.character == DialogueSystem.Character.Squire){
            King.SetActive(false);
            Squire.SetActive(true);
            SquireText.text = currDialogue.Dialogue;
        }

        if(currDialogue.SceneToLoad!=String.Empty){
            Dialogue.CurrentChapter++;
            Dialogue.CurrentDialogue=0;
            SceneManager.LoadScene(currDialogue.SceneToLoad);
        }
    }
}
