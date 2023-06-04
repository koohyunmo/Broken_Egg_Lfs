using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collision.collider);
            return;
        }

        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        effect.transform.localScale = new Vector3(1,1,1) * Random.Range(2f, 5f);
        Destroy(effect, 0.4f);
        Destroy(gameObject);
    }
}