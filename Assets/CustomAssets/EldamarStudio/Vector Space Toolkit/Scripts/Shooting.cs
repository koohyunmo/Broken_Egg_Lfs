using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform[] FirePoints;
    public GameObject BulletPrefab;
    public AudioSource MainAudioSource;
    public AudioClip LaserShootSound;
    public float BulletForce = 150f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        for (int i = 0; i < FirePoints.Length; ++i)
        {
            GameObject bullet = Instantiate(BulletPrefab, FirePoints[i].position, FirePoints[i].rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(FirePoints[i].up * BulletForce, ForceMode2D.Impulse);
        }

        MainAudioSource.clip = LaserShootSound;
        MainAudioSource.Play();
    }
}