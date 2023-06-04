using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFire : MonoBehaviour
{
    public float deg;
    public float turretSpeed;
    public GameObject turret;
    public GameObject jamBullet;


    private void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            if (deg >= 180)
            {
                deg = 180;
                return;
            }


            deg = deg + Time.deltaTime * turretSpeed;
            float rad = deg * Mathf.Deg2Rad;
            turret.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            turret.transform.eulerAngles = new Vector3(0, 0, deg);
        }else if(Input.GetKey(KeyCode.E))
        {
            if(deg <= 0)
            {
                deg = 0;
                return;
            }

            deg = deg - Time.deltaTime * turretSpeed;
            float rad = deg * Mathf.Deg2Rad;
            turret.transform.localPosition = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            turret.transform.eulerAngles = new Vector3(0, 0, deg);
        }


        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = Instantiate(jamBullet,gameObject.transform);
            go.transform.localPosition = turret.transform.localPosition;
        }
    }
}
