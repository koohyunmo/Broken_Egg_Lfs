using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCurveLineRenderer : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public Transform point3;
    public LineRenderer lineRenderer;
    public float vertexCount = 12;
    [Range(-10,10)]public float point2Ypositio = 2;



    private void Update()
    {
        point2.transform.position = new Vector3((point1.transform.position.x + point3.transform.position.x)/2, point2Ypositio, (point1.transform.position.z + point3.transform.position.z) / 2);
        List<Vector3> pointList = new List<Vector3>();

        for (float ratio = 0; ratio<=1; ratio += 1/vertexCount)
        {
            Vector3 tangent1 = Vector3.Lerp(point1.position, point2.position, ratio);
            Vector3 tangent2 = Vector3.Lerp(point2.position, point3.position, ratio);
            Vector3 curve = Vector3.Lerp(tangent1, tangent2, ratio);
            pointList.Add(curve);
        }

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }
}
