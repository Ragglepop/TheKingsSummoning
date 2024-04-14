using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class FlowerGameScript : MonoBehaviour
{

    // Array of upbuttons
    public GameObject[] upButtons;
    public GameObject[] downButtons;
    public Sprite[] flowerSprites;

    public GameObject BaseFlower;
    private FlowerState[] flowers;
    private const string FLOWER_PATH = "Scenes/FlowerArrangement/Sprites/";
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
        flowers = new FlowerState[5];
        for (int i = 0; i < 5; i++)
        {
            flowers[i] = new FlowerState(
                type: i,
                flower: Instantiate(BaseFlower, new Vector3(-3.3f + i * 1.65f, 0, 0), quaternion.identity)
            );
            flowers[i].flower.GetComponent<SpriteRenderer>().sprite = flowerSprites[flowers[i].type];

            // Set the up and down buttons
            Debug.Log("Setting up button " + i);
            int temp = i;
            upButtons[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(temp, true));
            downButtons[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(temp, false));
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
            flowers[index].type = (flowers[index].type + 1) % 5;
        }
        else
        {
            flowers[index].type = (flowers[index].type + 4) % 5;
        }

        GameObject flower = flowers[index].flower;
        flower.GetComponent<SpriteRenderer>().sprite = flowerSprites[flowers[index].type];
    }
}
