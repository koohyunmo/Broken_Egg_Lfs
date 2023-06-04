using UnityEngine;

public class DestroyAudio : MonoBehaviour
{
    void Start()
    {
        // Get audio clip lenght
        float destroyTimeout = GetComponent<AudioSource>().clip.length;

        // Destroy object after timeout
        Destroy(gameObject, destroyTimeout);
    }
}