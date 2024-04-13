using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    public Camera MCamera;
    public GameObject brush;
    private bool currentlyDrawing;
    LineRenderer currentLineRenderer;

    Vector2 lastPos;

    public static event Action<Vector2> CreatedPos;
    public static event Action JustFinshedDrawing;

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    private void Draw(){
        //First click
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            currentlyDrawing = true;
            CreateBrush();
        }

        //Hold
        if(Input.GetKey(KeyCode.Mouse0))
        {
            currentlyDrawing = true;
            Vector2 mousePos = GetMousePos();

            if(mousePos!=lastPos){
                AddAPoint(mousePos);
                lastPos = mousePos;
            }
        }else //No mouse press
        {
            if(currentlyDrawing){ //Just released mouse
                JustFinshedDrawing?.Invoke();
                currentlyDrawing = false;
            }
        }
    }

    private void CreateBrush(){
        if(currentLineRenderer!=null){
            Destroy(currentLineRenderer.gameObject);
        }

        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = GetMousePos();

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

        FireCreatePosEvent(mousePos);
    }

    private void AddAPoint(Vector2 pointPos){
        currentLineRenderer.positionCount++;
        int posIndex = currentLineRenderer.positionCount-1;
        currentLineRenderer.SetPosition(posIndex, pointPos);
        FireCreatePosEvent(pointPos);
    }

    private void FireCreatePosEvent(Vector2 pos){
        CreatedPos?.Invoke(pos);
    }

    private Vector2 GetMousePos(){
        return MCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
