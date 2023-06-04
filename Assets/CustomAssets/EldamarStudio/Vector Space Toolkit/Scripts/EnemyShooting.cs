using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform[] FirePoints;
    public GameObject BulletPrefab;
    public AudioSource MainAudioSource;
    public AudioClip LaserShootSound;
    public float BulletForce = 150f;
    public float FireRate = 150f;
    float NextFire = 1f;


    void Update()
    {
        if (Time.time > NextFire)
        {
            NextFire = Time.time + FireRate;

            for (int i = 0; i < FirePoints.Length; ++i)
            {
                GameObject clone = Instantiate(BulletPrefab, FirePoints[i].position, FirePoints[i].rotation);
                Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
                rb.AddRelativeForce(Vector2.up * BulletForce, ForceMode2D.Impulse);
            }
        }
    }
}