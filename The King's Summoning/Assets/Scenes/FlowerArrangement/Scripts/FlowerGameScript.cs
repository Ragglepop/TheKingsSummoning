using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class FlowerGameScript : MonoBehaviour
{

    // Array of upbuttons
    public GameObject[] upButtons;
    public GameObject[] downButtons;
    public GameObject checkButton;

    public Sprite[] flowerSprites;

    public GameObject BaseFlower;
    private FlowerState[] flowers;
    private int[] correctArrangement = { 0, 1, 0, 0, 1 };

    private const int FLOWER_COUNT = 5;

    struct FlowerState
    {
        public int type;
        public GameObject flower;

        public FlowerState(int type, GameObject flower)
        {
            this.type = type;
            this.flower = flower;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Create 5 base flowers
        flowers = new FlowerState[FLOWER_COUNT];
        for (int i = 0; i < FLOWER_COUNT; i++)
        {
            correctArrangement[i] = UnityEngine.Random.Range(0, flowerSprites.Length);

            flowers[i] = new FlowerState(
                type: i % flowerSprites.Length,
                flower: Instantiate(BaseFlower, new Vector3(-3.3f + i * 1.65f, 0, 0), quaternion.identity)
            );
            flowers[i].flower.GetComponent<SpriteRenderer>().sprite = flowerSprites[flowers[i].type];

            // Set the up and down buttons
            Debug.Log("Setting up button " + i);
            int temp = i;
            upButtons[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(temp, true));
            downButtons[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(temp, false));
        }

        // Set the check button
        checkButton.GetComponent<Button>().onClick.AddListener(OnCheckButtonClicked);
    }

    void resetflowers() 
    {
        for (int i = 0; i < FLOWER_COUNT; i++)
        {
            flowers[i].type = i % flowerSprites.Length;
            flowers[i].flower.GetComponent<SpriteRenderer>().sprite = flowerSprites[flowers[i].type];
            flowers[i].flower.GetComponent<SpriteRenderer>().color = Color.red;

            correctArrangement[i] = UnityEngine.Random.Range(0, flowerSprites.Length);
        }
    }
    // Update is called once per frame
    void Update()
    {
    }

    // Called when the player reaches the end of the level
    public void OnButtonClicked(int index, bool isUp)
    {
        Debug.Log("Button clicked: " + index + " " + isUp);
        // Change the flower sprite to a different sprite
        if (isUp)
        {
            flowers[index].type = (flowers[index].type + 1) % flowerSprites.Length;
        }
        else
        {
            flowers[index].type = (flowers[index].type + flowerSprites.Length - 1) % flowerSprites.Length;
        }

        GameObject flower = flowers[index].flower;
        flower.GetComponent<SpriteRenderer>().sprite = flowerSprites[flowers[index].type];
    }

    public void OnCheckButtonClicked()
    {
        Debug.Log("Check button clicked");
        // Check if the flowers are in the correct order
        bool isCorrect = true;
        for (int i = 0; i < FLOWER_COUNT; i++)
        {
            if (flowers[i].type != correctArrangement[i])
            {
                isCorrect = false;

                // Check if the type is in the arrangement
                if (Array.IndexOf(correctArrangement, flowers[i].type) != -1)
                {
                    // Set Flower colour to red
                    // Correct color Wrong position
                    flowers[i].flower.GetComponent<SpriteRenderer>().color = Color.blue;
                }
                else
                {
                    // Set Flower colour to green
                    // Wrong color wrong position
                    flowers[i].flower.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else {
                // Set Flower colour to green
                // Correct color correct position
                flowers[i].flower.GetComponent<SpriteRenderer>().color = Color.green;
            }

        }

        if (isCorrect)
        {
            victory();
        }
        else
        {
            Debug.Log("Incorrect!");
        }
    }

    public void victory()
    {
        Debug.Log("Victory!");
        StartCoroutine(WaitThenVictory());
    }

    public IEnumerator WaitThenVictory(){
        yield return new WaitForSeconds(4);
        //LevelLoader.LoadDialogueLevel();
    }
}
