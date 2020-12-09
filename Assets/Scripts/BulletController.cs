using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BulletController : MonoBehaviour
{
    public GameObject explosion;
    public float speed;
    public float damage;

    GameController gameController;

    // script for the bullet
    // Start is called before the first frame update
    void Start()
    {
        //destroy bullet after 20 seconds
        Destroy(gameObject, 20);
        Rigidbody r = GetComponent<Rigidbody>();
        //move bullet in a single direction
        r.velocity = transform.forward * speed;
        //find GameController to adjust health on hit
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //if a player or enemy collides with bullet, explode
        if (!collision.collider.CompareTag("Bullet"))
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                Explode();
            }
            else if (collision.collider.CompareTag("Player"))
            {
                Explode();
                gameController.PlayerHit(damage);
            }
            Destroy(gameObject);
        }
    }
    void Explode()
    {
        //Play explosion sound and particle system
        GameObject explosionEffect = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionEffect.GetComponent<AudioSource>().Play();
        explosionEffect.GetComponent<ParticleSystem>().Play();
    }
}
