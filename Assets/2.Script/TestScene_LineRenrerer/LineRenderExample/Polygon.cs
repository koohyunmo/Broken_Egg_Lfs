using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour
{

    [Range(3, 100)] public int polygonPoints = 3;
    [Min(0.1f)] public float radius = 3;
    private LineRenderer lineRenderer;

        private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
    }

    private void Update()
    {
        Play();
    }

    private void Play()
    {
        lineRenderer.positionCount = polygonPoints;
        float anglePerStep = 2 * Mathf.PI * ((float)1 / polygonPoints);

        for(int i = 0; i < polygonPoints; ++i)
        {
            Vector2 point = Vector2.zero;
            float angle = anglePerStep * i;
            point.x = Mathf.Cos(angle) * radius;
            point.y = Mathf.Sin(angle) * radius;

            lineRenderer.SetPosition(i, point);
        }
    }
}
