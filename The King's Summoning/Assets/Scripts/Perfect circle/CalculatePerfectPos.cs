using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CalculatePerfectPos : MonoBehaviour
{

    public float scaleFactor = 5f;
    public LinePositions linePositions;
    public float StartCellSize;
    public float CellSize;
    private float leftMostPoint;
    private float topMostPoint;
    private float rightMostPoint;
    private float bottomMostPoint;
    private Vector2[,] pointsMatrix;
    public float CirclePercentage;
    public Vector2 BestPoint;
    public float totalCells = 10;
    public int TotalPasses;
    public GameObject circle;
    private int currentPass;
    private Vector2 startingTopLeft;
    private Vector2 startingBottomRight;
    private Vector2 startPos;
    private Vector2 endPos;

    // Start is called before the first frame update
    void Start()
    {
        Drawing.JustFinshedDrawing += CalcCirclePercentage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CircleIsBigEnough(){
        return Vector2.Distance(startingTopLeft, startingBottomRight)>4f;
    }

    public bool CircleFinished(){
        return Vector2.Distance(startPos, endPos)<3f;
    }

    public void CalcCirclePercentage(){
        CellSize = StartCellSize;
        startPos = linePositions.linesPositions[0];
        endPos = linePositions.linesPositions[linePositions.linesPositions.Count-1];

        //Furthest points and cell size are used to create a points matrix
        CalcFurthestPoints();

        for(currentPass=0; currentPass<TotalPasses; currentPass++){
            //Create the points matrix
            CreatePointsMatrix();

            // Check which point in the points matrix is the best and stores them in the class variables
            CalcBestPoint();

            if(currentPass!=TotalPasses-1){
                // Use the best point to create new values for a smaller more specified matrix
                NewValues();
            }
        }

        linePositions.linesPositions.Clear();
        if(!CircleIsBigEnough()){
            Debug.Log("Draw a bigger circle");
        }else if(!CircleFinished()){
            Debug.Log("Close the circle");
        }else{
            Debug.Log($"Circle score: {CirclePercentage}%");
        }
    }

    private void CalcFurthestPoints(){
        leftMostPoint = float.MaxValue;
        topMostPoint = float.MinValue;
        rightMostPoint = float.MinValue;
        bottomMostPoint = float.MaxValue;

        foreach (Vector2 point in linePositions.linesPositions){
            if(point.x<leftMostPoint){
                leftMostPoint = point.x;
            }
            if(point.y>topMostPoint){
                topMostPoint = point.y;
            }
            if(point.x>rightMostPoint){
                rightMostPoint = point.x;
            }
            if(point.y<bottomMostPoint){
                bottomMostPoint = point.y;
            }
        }

        startingTopLeft = new Vector2(leftMostPoint,topMostPoint);
        startingBottomRight = new Vector2(rightMostPoint,bottomMostPoint);
    }

    private void CreatePointsMatrix(){
        float halfCell = CellSize/2f;

        float width = rightMostPoint - leftMostPoint;
        float height = topMostPoint - bottomMostPoint;
        int totalRows = Mathf.FloorToInt((height/CellSize));
        int totalColumns = Mathf.FloorToInt((width/CellSize));

        pointsMatrix = new Vector2[totalRows,totalColumns];
        for(int rowI=0; rowI<totalRows; rowI++){
            for(int colI=0; colI<totalColumns; colI++){
                pointsMatrix[rowI, colI] = new Vector2(leftMostPoint+colI*CellSize+halfCell, topMostPoint-rowI*CellSize-halfCell);
                Instantiate(circle.transform, pointsMatrix[rowI, colI], Quaternion.identity);
            }
        }
    }

    private float CalcCirclePercentageAtPoint(Vector2 centrePoint){
        float totalPoints = linePositions.linesPositions.Count;
        float totalDistance=0;
        foreach (Vector2 point in linePositions.linesPositions){
            totalDistance += Vector2.Distance(centrePoint, point);
        }

        float radius = totalDistance/totalPoints;

        float totalFraction=0;
        foreach(Vector2 point in linePositions.linesPositions){
            float currentDistance = Vector2.Distance(centrePoint, point);
            totalFraction+=MathF.Min(currentDistance, radius)/Mathf.Max(currentDistance, radius);
        }

        float CirclePercentage = totalFraction/totalPoints*100;

        return CirclePercentage;
    }

    private void CalcBestPoint(){
        Vector2 bestPoint=new Vector2();
        float bestPercentage=0;

        foreach(Vector2 point in pointsMatrix){
            float currPerc = CalcCirclePercentageAtPoint(point);
            if(currPerc>bestPercentage){
                bestPercentage = currPerc;
                bestPoint=point;
            }
        }

        BestPoint = bestPoint;
        GameObject c = Instantiate(circle, bestPoint, Quaternion.identity);
        c.GetComponent<SpriteRenderer>().color = Color.red;
        CirclePercentage=Mathf.Pow(bestPercentage/100f, scaleFactor)*100f;
    }

    private void NewValues(){
        float halfCell = CellSize/2f;
        leftMostPoint = BestPoint.x-halfCell;
        topMostPoint = BestPoint.y+halfCell;
        rightMostPoint = BestPoint.x+halfCell;
        bottomMostPoint = BestPoint.y-halfCell;

        CellSize = CellSize/10f;
        
        GameObject a = Instantiate(circle, new Vector2(leftMostPoint,topMostPoint), Quaternion.identity);
        a.GetComponent<SpriteRenderer>().color = Color.blue;
        a.name="top left";
        GameObject b = Instantiate(circle, new Vector2(rightMostPoint,topMostPoint), Quaternion.identity);
        b.GetComponent<SpriteRenderer>().color = Color.blue;
        b.name="top right";

        GameObject c = Instantiate(circle, new Vector2(rightMostPoint,bottomMostPoint), Quaternion.identity);
        c.GetComponent<SpriteRenderer>().color = Color.blue;
        c.name="bot right";

        GameObject d = Instantiate(circle, new Vector2(leftMostPoint,bottomMostPoint), Quaternion.identity);
        d.GetComponent<SpriteRenderer>().color = Color.blue;
        d.name="bot left";
    }
}
