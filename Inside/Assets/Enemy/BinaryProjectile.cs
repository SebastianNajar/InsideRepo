using TMPro;
using UnityEngine;

public class BinaryProjectile : MonoBehaviour
{
    public float speed;
    public GameObject explosion;

    private void Start()
    {
        Destroy(gameObject, 13);
    }
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            GameObject obj = Instantiate(explosion, collision.transform.position, collision.transform.rotation);
            Destroy(gameObject);
        }
    }
}
