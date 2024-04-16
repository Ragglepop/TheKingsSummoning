using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Script : MonoBehaviour
{

    public GameObject Player;
    public GameObject BaseLog;

    public float logSpawnInterval = 10.0f;
    private float initLogSpawnIntercal;
    public float IncreaseSpeedInterval;
    public float IncreaseSpeedAmount;
    private float TimeOfLastIncrease;
    public static float LogSpeed=1.75f;
    private float initSpeed;

    public Button blueButton, redButton, greenButton, yellowButton, purpleButton; 

    public int stream_count;
    private int level = 0;
    private List<Stream> streams = new List<Stream>();
    struct Stream
    {
        public float y;
        public float speed;
        public GameObject log;
    }
    private GameObject playerLog = null;
    private float lastLogSpawnTime = 0f;
    private const float WAVE_WIDTH = 2.5f;
    private bool streamPaused;
    private int pauseCount;
    public GameObject InstructionsObject;
    public GameObject ContinueObject;
    public GameObject RetryObject;
    public static event Action OnPauseStream;
    public static event Action OnUnpauseStream;

    // Start is called before the first frame update
    void Start()
    {
        PauseStream();
        initSpeed = LogSpeed;
        initLogSpawnIntercal=logSpawnInterval;
        Debug.Log($"Log speed:{LogSpeed}");

        InstructionsObject.SetActive(true);
        ContinueObject.SetActive(false);
        RetryObject.SetActive(false);

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

        btn = yellowButton.GetComponent<Button>();
        btn.onClick.AddListener(OnYellowButtonClicked);

        btn = purpleButton.GetComponent<Button>();
        btn.onClick.AddListener(OnPurpleButtonClicked);

        Player = Instantiate(Player, new Vector3(0, 0, 0), quaternion.identity);

        // Focus camera on player
        Camera.main.transform.position = new Vector3(0, Player.transform.position.y, -10);
    }

    // Update is called once per frame
    void Update()
    {
        if(streamPaused){
            return;
        }

        if(Time.time>TimeOfLastIncrease+IncreaseSpeedInterval){
            LogSpeed+=IncreaseSpeedAmount;
            TimeOfLastIncrease = Time.time;
            logSpawnInterval-=.14f;
        }

        for (int i = 0; i < streams.Count; i++)
        {
            Stream stream = streams[i];

            if (Time.time - lastLogSpawnTime > logSpawnInterval && stream.log == null && level <= i)
            {
                lastLogSpawnTime = Time.time;
                Vector3 position = new  Vector3(12, stream.y, 9);
                stream.log = Instantiate(BaseLog, position, quaternion.identity);
                stream.log.GetComponent<LogBehavior>().speed = stream.speed;
                streams[i] = stream; // Replace the old Stream object with the new one
            }

            // Prune of screen logs
            GameObject log = stream.log;
            if (stream.log != null && log.transform.position.x < -12)
            {   
                log.GetComponent<LogBehavior>().DestroyLog();
                if (playerLog == log)
                {
                    PauseStream();
                }
            }
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

        // Check if the player has moved off screen 
        if (Player.transform.position.x < -11 || Player.transform.position.x > 11)
        {
            //Player fell into water
            PauseStream();
        }
    }

    // Called when the player reaches the end of the level
    public void victory()
    {
        Debug.Log("Victory!");
        playerLog = null;
        level++;
        ContinueObject.SetActive(true);
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
    }

    public void OnRedButtonClicked()
    {
        moveToLogByColor(LogBehavior.LogColor.Red);
    }

    public void OnGreenButtonClicked()
    {
        moveToLogByColor(LogBehavior.LogColor.Green);
    }

    public void OnYellowButtonClicked()
    {
        Debug.Log("Yellow");
        moveToLogByColor(LogBehavior.LogColor.Yellow);
    }

    public void OnPurpleButtonClicked()
    {
        Debug.Log("Purple");
        moveToLogByColor(LogBehavior.LogColor.Purple);
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
        if(log==null) return;
        bool correctColor = log.GetComponent<LogBehavior>().color == color;
        if (correctColor)
        {
            level++;
            playerLog = log;
            if(level>=streams.Count){
                redButton.onClick.RemoveAllListeners();
                greenButton.onClick.RemoveAllListeners();
                blueButton.onClick.RemoveAllListeners();
                yellowButton.onClick.RemoveAllListeners();
                purpleButton.onClick.RemoveAllListeners();
                StartCoroutine(AutoJumpWithDelay());
            }
        } else
        {
            PauseStream();
        }
    }

    private IEnumerator AutoJumpWithDelay(){
        yield return new WaitForSeconds(.2f);
        moveToLogByColor(LogBehavior.LogColor.Red);
    }

    public void PauseStream(){
        if(pauseCount>0){
            LogSpeed = 0;
        }
        streamPaused = true;
        Player.transform.position = new Vector3(0,999,Player.transform.position.z);
        Debug.Log("Pausing stream");
        RetryObject.SetActive(true);
        OnPauseStream?.Invoke();
        pauseCount++;
    }

    public void UnpauseStream(){
        LogSpeed = initSpeed;
        logSpawnInterval = initLogSpawnIntercal;
        streamPaused = false;
        RetryObject.SetActive(false);
        InstructionsObject.SetActive(false);
        resetStream();
        OnUnpauseStream?.Invoke();
        TimeOfLastIncrease=Time.time;
        lastLogSpawnTime=Time.time;
    }
}
