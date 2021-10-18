using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    GameObject bullet;

    public GameObject [] projectile;
    int currentProjectile = 0;

    public float bulletSpeed = 10f;

    Vector2 PlayerDirection, PlayerPosition;
    bool playerIsMoving;

    public float firingPeriod = 2f;

    Camera myCam;

    Coroutine firingCoroutine, firingToMouseCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        myCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        bullet = projectile[currentProjectile];
    }

    // Update is called once per frame
    void Update()
    {
        //select firing projectile
        SelectBullet();

        //get player direction 
        PlayerDirection = GetComponent<MovementNew>().shootDirection();
        //get player position relative to screen point
        PlayerPosition = new Vector2(myCam.WorldToScreenPoint(transform.position).x - Screen.width/2, myCam.WorldToScreenPoint(transform.position).y - Screen.height/2);
        //check if player is moving
        playerIsMoving = PlayerDirection == new Vector2(0, 0);
        //Shoot
        ShootToCursorPos();
    }

    /// <summary>
    /// Shoot using player's direction
    /// </summary>
    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            firingCoroutine = StartCoroutine(FireContinously());
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    /// <summary>
    /// Shoot using mouse position
    /// </summary>
    void ShootToCursorPos()
    {
        if(Input.GetMouseButtonDown(0))
        {
            firingToMouseCoroutine = StartCoroutine(FireToMouseContinously());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopCoroutine(firingToMouseCoroutine);
        }
    }

    /// <summary>
    /// Shoot to mouse position
    /// </summary>
    /// <returns></returns>
    IEnumerator FireToMouseContinously()
    {
        while (true)
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height / 2);
            //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Instantiate bullet
            GameObject projectile = Instantiate(bullet, transform.position, transform.rotation);

            //Rotate projectile according to mouse position
            float mouseAngle = Mathf.Atan2(mousePosition.y - PlayerPosition.y, mousePosition.x - PlayerPosition.x) * Mathf.Rad2Deg;
            projectile.transform.eulerAngles = new Vector3 (0,0,mouseAngle - 90);

            //Add velocity
            Vector2 shootDirection = new Vector2(mousePosition.x - PlayerPosition.x, mousePosition.y - PlayerPosition.y);
            projectile.GetComponent<Rigidbody2D>().velocity = shootDirection.normalized * bulletSpeed;

            //Firing rate
            yield return new WaitForSeconds(firingPeriod);
        }
    }

    /// <summary>
    /// Shoot in player's direction
    /// </summary>
    /// <returns></returns>
    IEnumerator FireContinously()
    {
        while (true)
        {
            //Calculate direction 
            bool playerIsMoving = PlayerDirection == new Vector2(0, 0);
            Vector2 shootDirection = playerIsMoving ? new Vector2(1, 0) * bulletSpeed : PlayerDirection * bulletSpeed;

            //Instantiate bullet
            GameObject projectile = Instantiate(bullet, transform.position, transform.rotation);

            //Rotate projectile according to player's direction
            projectile.transform.eulerAngles = playerIsMoving ? new Vector3(0, 0, 90) : rotateProjectile();

            //Add velocity
            projectile.GetComponent<Rigidbody2D>().velocity = shootDirection;

            //Fire rate
            yield return new WaitForSeconds(firingPeriod);
        }

    }

    /// <summary>
    /// Calculate projectile's rotation
    /// </summary>
    /// <returns></returns>
    Vector3 rotateProjectile()
    {
        if(PlayerDirection.x == 0)
        {
            return new Vector3(0, 0, 0);
        }
        else if (PlayerDirection.y == 0)
        {
            return new Vector3(0, 0, 90);
        }
        else return new Vector3(0, 0,180 - Mathf.Atan2(PlayerDirection.y, PlayerDirection.x) * Mathf.Rad2Deg);
    }


    void SelectBullet()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            currentProjectile++;
            if(currentProjectile >= projectile.Length){
                currentProjectile = 0;
            }
            bullet = projectile[currentProjectile];
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            currentProjectile--;
            if(currentProjectile < 0){
                currentProjectile = projectile.Length - 1;
            }
            bullet = projectile[currentProjectile];
        }
    }
    
}
