using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerfectCircleUI : MonoBehaviour
{
    public float target;
    public TMP_Text ScoreText;
    public TMP_Text TargetText;
    public TMP_Text HighScoreText;
    public TMP_Text Instruction;
    public GameObject ContinueButton;
    public CalculatePerfectPos circleStats;
    // Start is called before the first frame update
    void Start()
    {
        ContinueButton.SetActive(false);
        TargetText.text = $"Target: {target}";
        ScoreText.text = "";
        HighScoreText.text = "High score: ";
        CalculatePerfectPos.NotBigEnough+=()=>{ScoreText.text=$"Draw a bigger circle";};
        CalculatePerfectPos.NotClosed+=()=>{ScoreText.text=$"Draw a complete circle";};
        CalculatePerfectPos.SuccesfulCircle+=OnSuccessfulCircle;
        Drawing.JustFinshedDrawing+=()=>{Instruction.gameObject.SetActive(false);};
    }

    private void OnSuccessfulCircle(){
        ScoreText.text=$"Score: {circleStats.CirclePercentage}";
        HighScoreText.text=$"High score: {circleStats.HighScore}";
        if(circleStats.CirclePercentage>target){
            ContinueButton.SetActive(true);
        }
    }

    private void ShowNotBigEnough(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
