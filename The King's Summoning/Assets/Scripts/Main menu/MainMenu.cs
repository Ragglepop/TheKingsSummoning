using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public LevelLoader loader;
    public GameState state;
    public void StartGame(){
        state.CurrentChapter = 0;
        state.CurrentDialogue = 0;
        loader.LoadLevel("Dialogue");
    }

    public void ContinueGame(){
        state.CurrentChapter = 0;
        state.CurrentDialogue = 0;
        if(state.LastLoadedLevel!=String.Empty){
            loader.LoadLevel(state.LastLoadedLevel);
        }else{
            loader.LoadLevel("Dialogue");
        }
    }
}
