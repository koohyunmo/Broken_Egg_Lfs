using UnityEngine;

public class MeteorMovment : MonoBehaviour
{
    public float MoveSpeed = 50f;

    void Start()
    {
        Destroy(gameObject, 20f);
    }

    void Update()
    {
        transform.position += new Vector3((MoveSpeed * -1) * Time.deltaTime, (MoveSpeed * -1) * Time.deltaTime, 0);
    }
}