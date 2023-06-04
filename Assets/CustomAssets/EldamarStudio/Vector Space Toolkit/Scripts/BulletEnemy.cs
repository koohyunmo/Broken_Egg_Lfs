using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public GameObject hitEffect;

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        effect.transform.localScale = new Vector3(1,1,1) * Random.Range(2f, 5f);
        Destroy(effect, 0.4f);
        Destroy(gameObject);
    }
}