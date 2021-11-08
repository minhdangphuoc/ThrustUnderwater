using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosionVFX;
    public float durationOfExplosion = 1f;

    public int ammoCount = 1;

    Rigidbody2D myRB;

    bool shooting;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        myRB = GetComponent<Rigidbody2D>();
        if(myRB.velocity.magnitude <= Vector2.one.magnitude) processExpolsion();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Game Wall" || collision.tag == "Enemy")
        {
            processExpolsion();
        }
    }


    void processExpolsion()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
    }
}
