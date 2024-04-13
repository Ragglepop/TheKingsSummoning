using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePositions : MonoBehaviour
{
    public List<Vector2> linesPositions;
    public float AddPointDelay;

    // More positions will make performance worse, but increase accuracy
    [Tooltip("The amount of positions should more or less be this amount")]
    public int estimatedTotalPositions;
    private float TimeOfLastPos;
    private Vector2 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        linesPositions = new List<Vector2>();
        Drawing.CreatedPos += TryAddPos;
        lastPos=new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TryAddPos(Vector2 pos){
        if(Vector2.Distance(lastPos, pos)>.7f)
        {
            TimeOfLastPos = Time.time;
            lastPos=pos;
            linesPositions.Add(pos);
        }
    }

    // Reduces positions to a smaller amount to make calcs less expensive
    private void ReduceLinePositions(){
        if(linesPositions.Count < estimatedTotalPositions){
            return;
        }

        int  eliminateEveryX = Mathf.FloorToInt(linesPositions.Count/estimatedTotalPositions);
        for(int i=0; i<linesPositions.Count; i++){
            if(i%eliminateEveryX == 0){
                linesPositions.Remove(linesPositions[i]);
            }
        }
    }
}
