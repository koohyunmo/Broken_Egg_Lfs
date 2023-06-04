using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererSinWave : MonoBehaviour
{
    
    [SerializeField] public float start = 0;
    [SerializeField] public float end = 0;
    [SerializeField] [Range(5, 50)] public int points = 5;
    [SerializeField] [Min(1)] public float amplitude = 1;
    [SerializeField] [Min(1)] public float frequency = 1;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Play();
    }

    private void Play()
    {
        lineRenderer.positionCount = points;

        for(int i =0; i < points; ++i)
        {
            float t = (float)i / (points - 1);

            float x = Mathf.Lerp(start, end, t);

            float y = amplitude * Mathf.Sin(2 * Mathf.PI * t * frequency);

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }

    }
}
