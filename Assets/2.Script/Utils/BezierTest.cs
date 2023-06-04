using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class BezierTest : MonoBehaviour
{
    public GameObject _go;
    [Range(0, 1)]

    public float test;
    bool re;

    public Vector3 _p1;
    public Vector3 _p4;
    public Vector3 _p3;
    public Vector3 _p2;

    public Vector3 BezierFunc(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float value)
    {

        Vector3 A = Vector3.Lerp(p1, p2, value); 
        Vector3 B = Vector3.Lerp(p2, p3, value); 
        Vector3 C = Vector3.Lerp(p3, p4, value);

        Vector3 D = Vector3.Lerp(A, B, value);
        Vector3 E = Vector3.Lerp(B, C, value);

        Vector3 F = Vector3.Lerp(D, E, value);

        return F;

    }

    private void FixedUpdate()
    {
        _go.transform.position = BezierFunc(_p1, _p2, _p3, _p4, test);

        if (re == false)
        {
            test += Time.deltaTime * 0.5f;
            if (test >= 1)
                re = true;
        }
        if(re == true)
        {
            test -= Time.deltaTime * 0.5f;
            if (test <= 0)
                re = false;
        }
        

    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(BezierTest))]
public class BezierEditor : Editor
{

    [SerializeField] int _count = 50;
    public  void OnSceneGUI()
    {
        BezierTest geneator = (BezierTest)target;

        geneator._p1 = Handles.PositionHandle(geneator._p1, Quaternion.identity);
        geneator._p2 = Handles.PositionHandle(geneator._p2, Quaternion.identity);
        geneator._p3 = Handles.PositionHandle(geneator._p3, Quaternion.identity);
        geneator._p4 = Handles.PositionHandle(geneator._p4, Quaternion.identity);

        Handles.color = Color.red;
        Handles.DrawLine(geneator._p1, geneator._p2);
        Handles.color = Color.yellow;
        Handles.DrawLine(geneator._p3, geneator._p4);


        
        for(float i = 0; i < _count; i++)
        {
            float valueBefore = i / 10;
            Vector3 before = geneator.BezierFunc(geneator._p1, geneator._p2, geneator._p3, geneator._p4, valueBefore);
            float valueAfter = (i + 1) / 10;
            Vector3 after = geneator.BezierFunc(geneator._p1, geneator._p2, geneator._p3, geneator._p4, valueAfter);

            Handles.color = Color.green;
            Handles.DrawLine(before, after);
        }
    }


}
#endif




