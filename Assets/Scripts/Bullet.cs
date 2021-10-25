using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosionVFX;
    public float durationOfExplosion = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Game Wall" || collision.tag == "Enemy")
        {
            Destroy(gameObject);
            GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
            Destroy(explosion, durationOfExplosion);
        }
    }
}
