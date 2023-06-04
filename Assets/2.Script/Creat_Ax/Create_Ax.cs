using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Ax : MonoBehaviour
{
    [SerializeField] [Range(1f,2000f)] float speed = 1f;

    private Rigidbody2D objectRigidbody2d;

	[SerializeField]
    private float rotSpeed = 100f;
    float randomX, randomY;

    Vector2 _vector2;
    [SerializeField]RectTransform pos;



    void Start()
    {


        objectRigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        pos = GetComponent<RectTransform>();



        randomX = Random.Range(-1f, 1f);
        randomY = Random.Range(-1f, 1f);

        _vector2 = new Vector2(randomX, randomY).normalized;

        objectRigidbody2d.AddForce(_vector2 * speed);
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, -rotSpeed * Time.deltaTime));

    }
}

