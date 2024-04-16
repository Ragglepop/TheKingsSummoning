using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class LogBehavior : MonoBehaviour
{
    public float speed = 1.0f;
    public LogColor color;
    public string[] ColourNames;
    public TMP_Text colourText;

    private static int iColor = 0;
    private static int MAX_COLOR = LogColor.GetValues(typeof(LogColor)).Length;

    public enum LogColor
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = Script.LogSpeed;

        int textIndex = UnityEngine.Random.Range(0, ColourNames.Length);
        colourText.text = ColourNames[textIndex];

        int colourIndex = UnityEngine.Random.Range(0, ColourNames.Length);
        color = (LogColor)colourIndex;
        // Set the log's color
        switch (color)
        {
            case LogColor.Red:
                colourText.color = Color.red;
                break;
            case LogColor.Green:
                colourText.color = Color.green;
                break;
            case LogColor.Blue:
                colourText.color = Color.blue;
                break;
            case LogColor.Yellow:
                colourText.color = Color.yellow;
                break;
            case LogColor.Purple:
                colourText.color = new Color(.53f,0.27f,0.9f);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        speed = Script.LogSpeed;
        // Move the log to the left
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    public void DestroyLog()
    {
        Destroy(gameObject);
    }
}
