using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    //Slider controlling ammo bar display
    public Slider ammoBar;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        ammoBar = ammoBar.GetComponent<Slider>();
        ammoBar.maxValue = player.GetComponent<Shooting>().maxAmmo;
        ammoBar.value = player.GetComponent<Shooting>().maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        //change bar value arcoding to ammo value
        ammoBar.value = player.GetComponent<Shooting>().Ammo;
    }
}
