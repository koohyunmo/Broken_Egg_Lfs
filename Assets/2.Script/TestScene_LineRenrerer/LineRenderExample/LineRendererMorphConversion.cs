using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererMorphConversion : MonoBehaviour
{
    public LineRenderer[] morphology;
    public float morphTime = 2;
    public LineRenderer lineRenderer;
    public int index = 0;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Play();
        }
    }

    private void Play()
    {
        StopAllCoroutines();
        StartCoroutine(MorphTo(morphology[index], morphTime));
        index = (index + 1) % morphology.Length;

    }

    private IEnumerator MorphTo(LineRenderer target, float time)
    {
        List<Vector3> startPoints = GetPoints(lineRenderer);
        List<Vector3> endPoints = GetPoints(target);

        int difference = startPoints.Count - endPoints.Count;
        if(difference > 0)
        {
            endPoints = AddFillerPoints(endPoints, difference);
        }
        else if(difference <0)
        {
            startPoints = AddFillerPoints(startPoints, -difference);
        }

        lineRenderer.positionCount = endPoints.Count;
        lineRenderer.loop = target;

        yield return StartCoroutine(OnMorphTo(startPoints, endPoints, time));

        lineRenderer.SetPositions(endPoints.ToArray());
    }

    private IEnumerator OnMorphTo(List<Vector3> start, List<Vector3> end, float time)
    {
        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;
            lineRenderer.SetPositions(LerpArray(start, end, percent));
            yield return null;
        }
    }

    private Vector3[] LerpArray(List<Vector3> start, List<Vector3> end, float t)
    {
        List<Vector3> temp = new List<Vector3>();
        for(int i =0;i< start.Count; ++i)
        {
            temp.Add(Vector3.Lerp(start[i], end[i], t));
        }


        return temp.ToArray();
    }

    private List<Vector3> AddFillerPoints(List<Vector3> points, int amount)
    {
        List<Vector3> originalPoints = new List<Vector3>(points);
        if(points.Count <= 1)
        {
            for(int i =0; i < amount; ++ i)
            {
                points.Add(Vector3.zero);
            }
            return points;
        }

        int selectionsAmount = points.Count - 1;
        int addedPoints = 0;

        for(int i=0; i< selectionsAmount; ++i)
        {
            int pointsToAdd = amount / selectionsAmount;

            if(amount % selectionsAmount > i)
            {
                pointsToAdd++;
            }

            for(int j= 0; j < pointsToAdd; ++j)
            {
                float percent = (float)1 / pointsToAdd * j;
                points.Insert(i + j + 1 + addedPoints, Vector3.Lerp(originalPoints[i], originalPoints[i + 1], percent));
            }

            addedPoints += pointsToAdd;
        }

        return points;
    }

    private List<Vector3> GetPoints(LineRenderer lineRenderer)
    {
        Vector3[] points = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(points);
        List<Vector3> pointsList = new List<Vector3>(points);

        return pointsList;
    }
}
