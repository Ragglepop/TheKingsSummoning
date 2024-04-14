using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Script : MonoBehaviour
{

    public GameObject Player;
    public GameObject BaseLog;

    public float logSpawnInterval = 10.0f;

    public Button blueButton, redButton, greenButton; 

    private int stream_count = 10;
    private int level = 0;
    private List<Stream> streams = new List<Stream>();
    struct Stream
    {
        public float y;
        public float speed;
        public GameObject log;
    }
    private GameObject playerLog = null;
    private float lastLogSpawnTime = -10.0f;
    private const float WAVE_WIDTH = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < stream_count; i++)
        {
            Stream stream = new Stream {
                y = i * WAVE_WIDTH,
                speed = 5.5f,
                log = null
            };
            streams.Add(stream);
        }

        Button btn = blueButton.GetComponent<Button>();
        btn.onClick.AddListener(OnBlueButtonClicked);

        btn = redButton.GetComponent<Button>();
        btn.onClick.AddListener(OnRedButtonClicked);

        btn = greenButton.GetComponent<Button>();
        btn.onClick.AddListener(OnGreenButtonClicked);

        Player = Instantiate(Player, new Vector3(0, 0, 0), quaternion.identity);

        // Focus camera on player
        Camera.main.transform.position = new Vector3(0, Player.transform.position.y, -10);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < streams.Count; i++)
        {
            Stream stream = streams[i];

            if (Time.time - lastLogSpawnTime > 1.2f && stream.log == null && level <= i)
            {
                lastLogSpawnTime = Time.time;
                Vector3 position = new  Vector3(12, stream.y, 9);
                stream.log = Instantiate(BaseLog, position, quaternion.identity);
                stream.log.GetComponent<LogBehavior>().speed = stream.speed;
                stream.log.GetComponent<LogBehavior>().setRandomColor();
                streams[i] = stream; // Replace the old Stream object with the new one
            }

            // Prune of screen logs
            GameObject log = stream.log;
            if (stream.log != null && log.transform.position.x < -12)
            {   
                log.GetComponent<LogBehavior>().DestroyLog();
                if (playerLog == log)
                {
                    resetStream();
                }
            }
        }

        // Check if the player has moved off screen 
        if (Player.transform.position.x < -10 || Player.transform.position.x > 10)
        {
            Debug.Log("Player fell in water");
            resetStream();
        }

        // Move the player to the log
        if (playerLog != null)
        {
            Player.transform.position = new Vector3(
                playerLog.transform.position.x,
                Player.transform.position.y,
                Player.transform.position.z
            );
        } else
        {
            Player.transform.position = new Vector3(
                0,
                Player.transform.position.y,
                Player.transform.position.z
            );
        }

        // Move the camera to follow the player
        Player.transform.position = new Vector3(Player.transform.position.x, level * WAVE_WIDTH - 2.6f, 4);
        Camera.main.transform.position = new Vector3(0, Player.transform.position.y + 1.75f, Camera.main.transform.position.z);
    }

    // Called when the player reaches the end of the level
    public void victory()
    {
        Debug.Log("Victory!");
        playerLog = null;
        level++;
    }

    public void resetStream() {
        level = 0;
        playerLog = null;

        for (int i = 0; i < streams.Count; i++)
        {
            Stream stream = streams[i];
            if (stream.log != null)
            {
                stream.log.GetComponent<LogBehavior>().DestroyLog();
                stream.log = null;
            }
            
        }
    }
    public void OnBlueButtonClicked()
    {
        moveToLogByColor(LogBehavior.LogColor.Blue);
        Debug.Log("Blue button clicked");
    }

    public void OnRedButtonClicked()
    {
        moveToLogByColor(LogBehavior.LogColor.Red);
        Debug.Log("Red button clicked");
    }

    public void OnGreenButtonClicked()
    {
        moveToLogByColor(LogBehavior.LogColor.Green);
        Debug.Log("Green button clicked");
    }

    private void moveToLogByColor(LogBehavior.LogColor color)
    {
        if (level >= streams.Count && playerLog != null)
        {
            // Player has reached the end of the level
            victory();

            return;
        } else if (level >= streams.Count)
        {
            return;
        }
        

        Stream stream = streams[level];
        GameObject log = stream.log;
        bool correctColor = log.GetComponent<LogBehavior>().color == color;
        if (correctColor)
        {
            level++;
            playerLog = log;
        } else
        {
            resetStream();
        }
    }
}
