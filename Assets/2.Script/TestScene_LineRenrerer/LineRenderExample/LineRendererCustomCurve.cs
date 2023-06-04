using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineRendererCustomCurve : MonoBehaviour
{

    public Transform start;
    public Transform end;
    public LineRenderer lineRenderer;
    public List<Vector3> pointsList = new List<Vector3>();

    GameObject go;
    [SerializeField]Vector2 mid;

    [Range(-500, 500)] public float vectorValue;





    private void Start()
    {



        mid = Vector2.Lerp(start.position, end.position, 0.5f);
        go = new GameObject { name = "mid" };
        go.AddComponent<Image>();
        go.transform.SetParent(gameObject.transform);
        go.transform.localPosition = mid;
        go.transform.localScale = new Vector3(1f, 1f, 1f);

        pointsList.Add(start.transform.localPosition) ;
        pointsList.Add(mid);
        pointsList.Add(end.transform.localPosition);



        lineRenderer.SetPosition(0, start.transform.localPosition);
        lineRenderer.SetPosition(1, mid);
        //lineRenderer.SetPosition(2, end.transform.localPosition);

    }

    private void Update()
    {
        mid.x = vectorValue;
        go.transform.localPosition = new Vector2(mid.x, go.transform.localPosition.y);

    }

}
