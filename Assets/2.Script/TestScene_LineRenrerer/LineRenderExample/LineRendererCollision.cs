using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class LineRendererCollision : MonoBehaviour
{
    public EdgeCollider2D edgeCollider2D;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        edgeCollider2D = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Play();
    }

    private void Play()
    {
        List<Vector2> edgePoints = new List<Vector2>();

        for(int i =0; i < lineRenderer.positionCount; ++i)
        {
            edgePoints.Add(lineRenderer.GetPosition(i));
        }
        edgeCollider2D.SetPoints(edgePoints);
    }
}
