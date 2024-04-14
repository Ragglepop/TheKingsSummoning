using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame(){
        GameState.instance.CurrentChapter = 0;
        GameState.instance.CurrentDialogue = 0;
        LevelLoader.Load("Dialogue");
    }

    public void ContinueGame(){
        GameState.instance.CurrentChapter = 0;
        GameState.instance.CurrentDialogue = 0;
        if(GameState.instance.LastLoadedLevel!=String.Empty){
            LevelLoader.Load(GameState.instance.LastLoadedLevel);
        }else{
            LevelLoader.Load("Dialogue");
        }
    }
}
