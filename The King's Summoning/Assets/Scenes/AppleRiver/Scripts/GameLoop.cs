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
        public float lastLogSpawnTime;
        public float maxLogSpawnInterval;
        public float minLogSpawnInterval;
        public List<GameObject> logs;
    }
    private GameObject playerLog = null;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < stream_count; i++)
        {
            Stream stream = new Stream {
                y = i * 1.5f,
                speed = 1.0f + (i + 1) / 4.0f,
                lastLogSpawnTime = UnityEngine.Random.Range(-2f, 2f),
                maxLogSpawnInterval = 10.0f,
                minLogSpawnInterval = 5.0f - i / 15.0f,
                logs = new List<GameObject>()
            };
            streams.Add(stream);
        }

        Button btn = blueButton.GetComponent<Button>();
        btn.onClick.AddListener(OnBlueButtonClicked);

        btn = redButton.GetComponent<Button>();
        btn.onClick.AddListener(OnRedButtonClicked);

        btn = greenButton.GetComponent<Button>();
        btn.onClick.AddListener(OnGreenButtonClicked);

        Player = Instantiate(Player, new Vector3(0, 1.5f, 0), quaternion.identity);

        // Focus camera on player
        Camera.main.transform.position = new Vector3(0, Player.transform.position.y, -10);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < streams.Count; i++)
        {
            Stream stream = streams[i];

            if (Time.time - stream.lastLogSpawnTime > stream.minLogSpawnInterval)
            {
                stream.lastLogSpawnTime = Time.time;
                Vector3 position = new  Vector3(15 + UnityEngine.Random.Range(-stream.speed / 1.2f, stream.speed / 1.2f), stream.y, 10);
                GameObject log = Instantiate(BaseLog, position, quaternion.identity);
                log.GetComponent<LogBehavior>().speed = stream.speed;
                log.GetComponent<LogBehavior>().setRandomColor();

                stream.logs.Add(log);
                streams[i] = stream; // Replace the old Stream object with the new one
            }

            // Prune of screen logs
            for (int j = 0; j < stream.logs.Count; j++)
            {
                GameObject log = stream.logs[j];
                if (log.transform.position.x < -12)
                {   
                    stream.logs.RemoveAt(j);
                    log.GetComponent<LogBehavior>().DestroyLog();
                }
            }
        }

        // Check if the player has moved off screen 
        if (Player.transform.position.x < -10 || Player.transform.position.x > 10)
        {
            Debug.Log("Player fell in water");
            playerLog = null;
            level = 0;
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
        Player.transform.position = new Vector3(Player.transform.position.x, level * 1.5f - 1.75f, 8);
        Camera.main.transform.position = new Vector3(0, Player.transform.position.y + 1.75f, Camera.main.transform.position.z);
    }

    // Called when the player reaches the end of the level
    public void victory()
    {
        Debug.Log("Victory!");
        playerLog = null;
        level++;
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
        float rightMostLog = -12;
        foreach (GameObject log in stream.logs)
        {
            bool correctColor = log.GetComponent<LogBehavior>().color == color;
            bool closeEnough = Mathf.Abs(log.transform.position.x - Player.transform.position.x) < 8f;
            bool logOnScreen = log.transform.position.x > -9;
            if (correctColor && closeEnough && logOnScreen)
            {
                if (log.transform.position.x > rightMostLog)
                {
                    rightMostLog = log.transform.position.x;
                    playerLog = log;
                }
            }
        }

        if (playerLog != null && rightMostLog > -12)
        {
            level++;
        }
    }


}
