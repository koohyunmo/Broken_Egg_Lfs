using UnityEngine;
using System.Collections;

public class RotateSpotlight : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.forward; // ȸ�� ���� �����մϴ�.

    void Update()
    {
        transform.Rotate(rotationAxis * Time.deltaTime); // ����Ʈ����Ʈ�� ȸ���մϴ�.
    }
}
