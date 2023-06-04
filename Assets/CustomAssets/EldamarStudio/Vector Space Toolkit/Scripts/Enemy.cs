using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject HitEffect;
    public AudioClip ExplosionSound;
    public int Health = 50;

    GameObject Player;
    Vector2 direction;
    float DistanceOffset = 70;
    float distance;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        direction = Player.transform.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        // Mesure distance between enemy and player
        distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance > DistanceOffset)
        {
            // Follow player
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 0.25f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is bullet
        if (Health > 0 && collision.collider.gameObject.CompareTag("HeroBullet"))
        {
            Damage();
        }
    }

    void Damage()
    {
        // Decrease object health
        Health -= 10;

        // If object has no health
        if (Health <= 0)
        {
            // Create explosion audio source object
            GameObject audioObject = new GameObject("Explosion Sound");

            // Add self destroy script to explosion audio source object
            audioObject.AddComponent<DestroyAudio>();

            // Play Explosion sound
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = ExplosionSound;
            audioSource.Play();

            // Create Explosion
            GameObject effect = Instantiate(HitEffect, transform.position, Quaternion.identity);
            effect.transform.localScale = new Vector3(1, 1, 1) * Random.Range(7f, 10f);

            // Destroy explosion after timeout
            Destroy(effect, 0.4f);

            // Destroy object
            Destroy(gameObject);
        }
    }
}