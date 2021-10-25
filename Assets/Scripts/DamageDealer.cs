using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damage = 100f;

    public float getDamage()
    {
        return damage;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
