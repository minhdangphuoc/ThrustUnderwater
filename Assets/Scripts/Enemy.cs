using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// This game object's health
    /// </summary>
    public float HP = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //get damage dealer
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        //if not a damage dealer then return
        if (!damageDealer) return;
        //decrease HP
        HP -= damageDealer.getDamage();
        //destroy game object
        if (HP <= 0) Destroy(gameObject);
    }
}
