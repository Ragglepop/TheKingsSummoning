using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerfectCircleUI : MonoBehaviour
{
    public float target;
    public TMP_Text ScoreText;
    public TMP_Text TargetText;
    public CalculatePerfectPos circleStats;
    // Start is called before the first frame update
    void Start()
    {
        TargetText.text = $"Target: {target}";
        ScoreText.text = "";
        CalculatePerfectPos.NotBigEnough+=()=>{ScoreText.text=$"Draw a bigger circle";};
        CalculatePerfectPos.NotClosed+=()=>{ScoreText.text=$"Draw a complete circle";};
        CalculatePerfectPos.SuccesfulCircle+=()=>{ScoreText.text=$"Score: {circleStats.CirclePercentage}";};
    }

    private void ShowNotBigEnough(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
