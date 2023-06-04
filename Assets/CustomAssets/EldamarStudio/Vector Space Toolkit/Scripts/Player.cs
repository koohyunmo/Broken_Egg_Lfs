using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public GameObject HitEffect;
    public AudioClip ExplosionSound;
    public Image HealthBar;
    public const float MaxHealth = 100;
    public float Health = 100;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Health <= 0)
            return;

        // Check if collision is bullet
        if (collision.collider.gameObject.CompareTag("EnemyBullet"))
        {
            Damage(10);
            return;
        }

        // Check if collision is asteroid
        if (collision.collider.gameObject.CompareTag("Asteroid"))
        {
            Damage(100);
        }
    }

    void Damage(int value)
    {
        // Decrease object health
        Health -= value;

        if (Health < 0)
            Health = 0;

        // Update progress bar
        HealthBar.fillAmount = Health / MaxHealth;

        // If object has no health
        if (Health == 0)
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

            // Destroy Player Sprite
            Destroy(GetComponent<SpriteRenderer>());

            // Reload scene after delay
            StartCoroutine(ReloadScene());
        }
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2f); // wait 2 sec

        // Reload scene
        SceneManager.LoadScene(0);
    }
}