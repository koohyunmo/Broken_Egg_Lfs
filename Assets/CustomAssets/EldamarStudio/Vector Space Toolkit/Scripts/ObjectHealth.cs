using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    public GameObject HitEffect;
    public AudioClip ExplosionSound;
    public int Health = 100;

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