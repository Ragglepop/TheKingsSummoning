using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragEvent : MonoBehaviour
{
    public static event Action<GameObject, Vector3> OnDragging;
    public static event Action<GameObject, Vector3> OnStopDragging;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector3 currPos;
    private bool dragging;
    public PositionShifter.FlowerType type;

    void OnMouseDown(){
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        currPos = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        dragging=true;

        OnDragging?.Invoke(gameObject, currPos);
    }

    void OnMouseUp(){
        if(dragging){
            OnStopDragging?.Invoke(gameObject, currPos);
        }

        dragging=false;
    }
}
