using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCreation : MonoBehaviour
{
    [SerializeField] private LineRenderer drawLine;
    private Vector2[] vertArray;


    private void Start() =>  vertArray = GenerateVerts.GenerateVertArray(3, 1, 0);
    private void Update() => DrawPolygon();
    private void DrawPolygon()
    {
        Vector2 startCorner = transform.TransformPoint(vertArray[0]);
        Vector2 previousCorner = startCorner;
        
        for(int i = 0; i < 3; i++)
        {
            Vector3 currentCorner = transform.TransformPoint(vertArray[i]);
            drawLine.SetPosition(i, currentCorner);
            
            int x = i + 1;
            if(x < drawLine.positionCount) drawLine.SetPosition(x, previousCorner);
            previousCorner = currentCorner;
        }
    }
}
