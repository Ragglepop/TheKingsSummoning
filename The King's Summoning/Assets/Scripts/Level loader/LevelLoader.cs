using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameState state;

    public void LoadLevel(string level){
        state.LastLoadedLevel=level;
        SceneManager.LoadScene(level);
    }

    public void LoadDialogueLevel(){
        state.CurrentDialogue=0;
        LoadLevel("Dialogue");
    }
}
