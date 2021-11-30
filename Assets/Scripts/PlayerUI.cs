using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    //slider controlling HP bar display
    public Slider healthBar;


    //Slider controlling ammo bar display
    public Slider ammoBar;


    GameObject player;
    Shooting playerShooting;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerShooting = player.GetComponent<Shooting>();
        
        //setting ammo bar
        ammoBar = ammoBar.GetComponent<Slider>();
        ammoBar.maxValue = playerShooting.maxAmmo;
        ammoBar.value = playerShooting.maxAmmo;

        //setting health bar
        healthBar = healthBar.GetComponent<Slider>();
        healthBar.maxValue = playerShooting.playerHP;
        healthBar.value = playerShooting.playerHP;


    }

    // Update is called once per frame
    void Update()
    {
        //change bar value arcoding to ammo value
        ammoBar.value = player.GetComponent<Shooting>().Ammo;

        //change bar value arcoding to HP value
        healthBar.value = player.GetComponent<Shooting>().playerHP;
    }
}
