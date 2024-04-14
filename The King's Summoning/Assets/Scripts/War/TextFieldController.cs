using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class TextFieldController : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text CurrentWord;
    public RectTransform TimeTransform;
    public float Interval;
    public float TimeOfLastWordChange;
    public string[] words;
    public EventSystem ESystem;
    private int wordIndex;
    private bool lost;

    // Start is called before the first frame update
    void Start()
    {
        inputField.onValueChanged.AddListener(OnInputChange);
        SetCurrentWord();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameComplete()){
            return;
        }
        
        float timeFraction = (Time.time-TimeOfLastWordChange)/Interval;
        Debug.Log(timeFraction);
        float width = 480-480*timeFraction;
        //Debug.Log(width);
        TimeTransform.sizeDelta = new Vector2(width,11.5f);

        if(Time.time > TimeOfLastWordChange+Interval){
            LoseGame();
        }

        ESystem.SetSelectedGameObject(inputField.gameObject);
    }

    private void OnInputChange(string s){
        if(GameComplete()){
            return;
        }

        if(s.ToUpper() == words[wordIndex]){
            TimeOfLastWordChange = Time.time;
            inputField.text = String.Empty;
            wordIndex++;
            SetCurrentWord();
        }
    }

    private void SetCurrentWord(){
        if(GameComplete()){
            CurrentWord.text = String.Empty;
        }else{
            CurrentWord.text = words[wordIndex];
        }
    }

    private bool GameComplete(){
        return wordIndex>=words.Length;
    }

    private void LoseGame(){
        Debug.Log("Lost");
        lost = true;
        wordIndex=0;
        SetCurrentWord();
        TimeOfLastWordChange = float.MaxValue-Interval;
    }

}
