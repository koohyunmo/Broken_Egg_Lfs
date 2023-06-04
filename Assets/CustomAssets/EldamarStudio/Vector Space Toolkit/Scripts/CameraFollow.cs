using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;
    public float smoothSpeed = 5f;

    void Update()
    {
        Vector3 targetPosition = new Vector3(Player.position.x, Player.position.y, transform.position.z);

        // Camera Smooth
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}