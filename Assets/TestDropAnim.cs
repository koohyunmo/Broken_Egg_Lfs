using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestDropAnim : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        
        StartCoroutine(DoTest());
    }


    private Vector3[] wayPoints;
    public Transform Target;

    IEnumerator DoTest()
    {

        wayPoints = new Vector3[4];

        while (true)
        {

            
            
            float random = Random.Range(-2, 2);


            Vector3 midle = (Target.position - transform.position)/2;

            Debug.DrawLine(transform.position, Target.position - transform.position, Color.blue);
            

            wayPoints.SetValue(transform.position, 0);
            wayPoints.SetValue(midle += new Vector3(random, 0,0), 1);
            wayPoints.SetValue(Target.position, 2);



            transform.DOPath(wayPoints, 3f, PathType.CatmullRom, PathMode.Sidescroller2D);

            yield return new WaitForSeconds(3f);
        }
    }

}
