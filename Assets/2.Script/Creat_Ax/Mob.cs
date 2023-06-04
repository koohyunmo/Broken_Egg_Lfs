using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creat_Ax : MonoBehaviour
{
    [SerializeField]
    private float startSpeed = 1;

    [SerializeField]
    private Rigidbody2D objectRigidbody2d;

	[SerializeField]
    private float rotSpeed = 100f;


    public int axdamge = 1;

	void Start()
    {
        float randomX, randomY;

        randomX = Random.Range(-0.1f, 0.1f);
        randomY = Random.Range(-0.1f, 0.1f);

        Vector2 vector2 = new Vector2(randomX, randomY);
        vector2 = vector2.normalized;
        
        objectRigidbody2d.AddForce(vector2 * startSpeed);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0,0, -rotSpeed * Time.deltaTime));
    }
}
