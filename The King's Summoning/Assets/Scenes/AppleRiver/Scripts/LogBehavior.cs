using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogBehavior : MonoBehaviour
{
    public float speed = 1.0f;
    public LogColor color;

    private static int iColor = 0;
    private static int MAX_COLOR = LogColor.GetValues(typeof(LogColor)).Length;

    public enum LogColor
    {
        Red,
        Green,
        Blue
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(3, 1.2f, 1);

        // Set the log's color
        switch (color)
        {
            case LogColor.Red:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case LogColor.Green:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case LogColor.Blue:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the log to the left
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    public void setRandomColor()
    {
        color = (LogColor) iColor;
        iColor = (iColor + 1) % MAX_COLOR;
    }

    public void DestroyLog()
    {
        Destroy(gameObject);
    }
}
