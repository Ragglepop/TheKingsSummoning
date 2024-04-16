using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class PositionShifter : MonoBehaviour
{
    public GameObject[] Flowers;
    public FlowerType[] CorrectFlowers;
    public int AmountInCorrectPos;
    public int TriesLeft;
    public TMP_Text AttemptsText;
    public TMP_Text AmountCorrectText;
    private bool started;
    public GameObject ContinueObject;
    public GameObject CheckObject;

    private enum Direction{
        None,
        Left,
        Right
    }

    public enum FlowerType{
        flower1=0,
        flower2=1,
        flower3=2,
        flower4=3,
        flower5=4
    }

    private Direction currentDirection;
    private UnityEngine.Vector3 prevMousePosition;
    
    private void Start(){
        ContinueObject.SetActive(false);
        DragEvent.OnDragging +=MoveFlower;

        ShuffleFlowers();
        ShuffleCorrectFlowers();
        UpdateCorrectAmount();
        started=true;
    }

    private void Update(){
        AmountCorrectText.text = $"Amount in correct position: {AmountInCorrectPos}";
        AttemptsText.text = $"Guesses left: {TriesLeft}";
    }

    private void MoveFlower(GameObject flower, UnityEngine.Vector3 pos){
        if(pos.x<-7){
            pos.x=-7;
        }
        if(pos.x>7){
            pos.x=7;
        }
        flower.transform.position = new UnityEngine.Vector2(pos.x,0);
    }

    private void ShuffleFlowers(){
        for(int i=0; i<10; i++){
            int i1 = UnityEngine.Random.Range(0,5);
            int i2 = UnityEngine.Random.Range(0,5);
            SwapFlowers(i1, i2);
        }
        
        void SwapFlowers(int idx1, int idx2){
            GameObject temp = Flowers[idx1];
            Flowers[idx1]=Flowers[idx2];
            Flowers[idx2]=temp;
        }

        for(int i=0;i<5;i++){
            GameObject flower = Flowers[i];
            flower.transform.position = new Vector2(-4+2*i,0);
        }
    }

    private void ShuffleCorrectFlowers(){
        for(int i=0; i<10; i++){
            int i1 = UnityEngine.Random.Range(0,5);
            int i2 = UnityEngine.Random.Range(0,5);
            SwapFlowers(i1, i2);
        }
        
        void SwapFlowers(int idx1, int idx2){
            FlowerType temp = CorrectFlowers[idx1];
            CorrectFlowers[idx1]=CorrectFlowers[idx2];
            CorrectFlowers[idx2]=temp;
        }
    }

    public void Check(){
        TriesLeft--;
        UpdateFlowers();
        UpdateCorrectAmount();
        if(AmountInCorrectPos==5){
            Victory();
        }else if(TriesLeft==0){
            Lose();
        }
    }

    private void UpdateFlowers(){
        Flowers = Flowers.OrderBy(v => v.transform.position.x).ToArray<GameObject>();
    }

    private void UpdateCorrectAmount(){
        int totalCorrect=0;
        for(int i=0; i<5; i++){
            GameObject flower = Flowers[i];
            if(flower.GetComponent<DragEvent>().type==CorrectFlowers[i]){
                totalCorrect++;
            }
        }
        AmountInCorrectPos=totalCorrect;
        Debug.Log("Amount in correct pos: "+AmountInCorrectPos);
    }

    private void Victory(){
        Debug.Log("Win");
        CheckObject.SetActive(false);
        ContinueObject.SetActive(true);
    }

    private void Lose(){
        Debug.Log("Lose");
        SceneManager.LoadScene("Flowers");
    }
}
