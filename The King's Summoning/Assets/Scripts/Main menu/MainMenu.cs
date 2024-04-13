using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public DialogueSystem dSystem;
    public void StartGame(){
        dSystem.CurrentChapter = 0;
        dSystem.CurrentDialogue = 0;
        SceneManager.LoadScene("Dialogue");
    }
}
