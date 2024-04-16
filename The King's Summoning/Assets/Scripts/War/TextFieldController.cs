using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using UnityEditor;

public class TextFieldController : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text CurrentWord;
    public RectTransform TimeTransform;
    public float Interval;
    public float TimeOfLastWordChange;
    public string[] words;
    public EventSystem ESystem;
    public Button RetryButton;
    public Button StartButton;
    public Button UnpauseButton;
    public Button ContinueButton;
    public GameObject Victory;
    public GameObject WordAndTime;
    public GameObject StartObject;
    public int wordIndex;
    private bool lost;
    public enum State{
        None,
        Playing,
        Paused,
        Retry,
        Complete
    }
    private State state;

    public static event Action OnPause;
    public static event Action OnLose;

    // Start is called before the first frame update
    void Start()
    {
        StartObject.SetActive(true);
        WordAndTime.SetActive(false);

        StartButton.onClick.AddListener(PlayGame);
        UnpauseButton.onClick.AddListener(PlayGame);
        RetryButton.onClick.AddListener(RetryGame);
        UnpauseButton.gameObject.SetActive(false);
        RetryButton.gameObject.SetActive(false);
        ContinueButton.gameObject.SetActive(false);

        inputField.onValueChanged.AddListener(OnInputChange);

        state = State.Paused;

        SetCurrentWord();
    }

    // Update is called once per frame
    void Update()
    {

        if(state!=State.Playing){
            return;
        }
        
        float timeFraction = (Time.time-TimeOfLastWordChange)/Interval;
        float width = 560-560*timeFraction;
        //Debug.Log(width);
        TimeTransform.sizeDelta = new Vector2(width,11.5f);

        if(Time.time > TimeOfLastWordChange+Interval){
            LoseGame();
        }

        ESystem.SetSelectedGameObject(inputField.gameObject);
    }

    private void OnInputChange(string s){
        if(state!=State.Playing){
            return;
        }

        if(s.ToUpper() == words[wordIndex]){
            TimeOfLastWordChange = Time.time;
            inputField.text = String.Empty;
            wordIndex++;
            SetCurrentWord();
            if(wordIndex==words.Length){
                ComepleteGame();
            }else{
                PauseGame();
            }
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
        ChangeState(State.Retry);
    }

    public void ChangeState(State newState){
        if(newState==state) return;
        if(newState==State.Playing){
            WordAndTime.SetActive(true);
        }else{
            WordAndTime.SetActive(false);
            ContinueButton.gameObject.SetActive(false);
        }
        if(newState==State.Playing){
            inputField.text = String.Empty;
            StartButton.gameObject.SetActive(false);
        }
        if(state==State.Paused && newState==State.Playing){
            UnpauseButton.gameObject.SetActive(false);
        }
        if(state==State.Paused && newState==State.Playing){
            TimeOfLastWordChange = Time.time;
        }
        if(state==State.Playing && newState==State.Paused){
            //UnpauseButton.gameObject.SetActive(true);
        }
        if(state==State.Retry && newState==State.Playing){
            StartButton.gameObject.SetActive(false);
            RetryButton.gameObject.SetActive(false);
            TimeOfLastWordChange = Time.time;
        }
        if(newState==State.Retry){
            OnLose?.Invoke();
            Debug.Log("Lost");
            StartCoroutine(WaitThenLose());
        }
        if(newState==State.Complete){
            ContinueButton.gameObject.SetActive(true);
            Victory.SetActive(true);
            RetryButton.gameObject.SetActive(false);
        }


        state=newState;
    }

    public void PlayGame(){
        ChangeState(State.Playing);
    }

    public void PauseGame(){
        ChangeState(State.Paused);
        OnPause?.Invoke();
    }

    public void RetryGame(){
        ChangeState(State.Playing);
    }

    public void ComepleteGame(){
        ChangeState(State.Complete);
    }

    private IEnumerator WaitThenLose(){
        yield return new WaitForSeconds(3);
        RetryButton.gameObject.SetActive(true);
        wordIndex=0;
        CurrentWord.text = words[wordIndex];
        TimeOfLastWordChange = float.MaxValue-Interval;
        StopAllCoroutines();
    }

    public void StartGame(){
        StartObject.SetActive(false);
    }
}
