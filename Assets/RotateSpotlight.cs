using UnityEngine;
using System.Collections;

public class RotateSpotlight : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.forward; // 회전 축을 정의합니다.

    void Update()
    {
        transform.Rotate(rotationAxis * Time.deltaTime); // 스포트라이트를 회전합니다.
    }
}
