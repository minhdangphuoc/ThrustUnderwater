using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shooting : MonoBehaviour
{
    GameObject bullet;

    public float PlayerHP = 100f;

    public GameObject [] projectile;
    int currentProjectile = 0;

    public float gunPower = 1000f;

    Vector2 PlayerDirection, PlayerPosition;
    bool playerIsMoving;

    Camera myCam;

    Transform gun;

    public int maxAmmo = 10;
    
    int ammo;
    public float reloadTime = 1f;
    bool shooting = false;

    Coroutine firingCoroutine, reloadCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
        gun = transform.GetChild(0);
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
        StartCoroutine(Shoot());

    }

    
    IEnumerator reload()
    {
        //yield return new WaitForSeconds(reloadTime);
        while(ammo < maxAmmo) {
            yield return new WaitForSeconds(reloadTime);
            ammo ++;
        }
    }

    public int Ammo
    {
        get {return ammo;}
    }

    public float playerHP
    {
        get {return PlayerHP;}
    }

    /// <summary>
    /// Shoot using player's direction
    /// </summary>
    IEnumerator Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(ammo >= bullet.GetComponent<Bullet>().ammoCount){
               shooting = true;
               firingCoroutine = StartCoroutine(FireContinously());        
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
               shooting = false;
               StopCoroutine(firingCoroutine);
               reloadCoroutine = StartCoroutine(reload());
        }

        if(shooting && reloadCoroutine != null) StopCoroutine(reloadCoroutine);
        
        yield return new WaitForSeconds(bullet.GetComponent<Bullet>().FiringPeriod);    
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
            Vector2 shootDirection = playerIsMoving ? new Vector2(1, 0) : PlayerDirection;

            //Instantiate bullet
            GameObject projectile = Instantiate(bullet, gun.transform.position, transform.rotation);

            //Rotate projectile according to player's direction
            projectile.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(PlayerDirection.y, PlayerDirection.x) * Mathf.Rad2Deg - 90);

            //Add velocity
            projectile.GetComponent<Rigidbody2D>().AddForce(shootDirection.normalized * gunPower, ForceMode2D.Impulse);
            ammo -= projectile.GetComponent<Bullet>().ammoCount;

            //Fire rate
            yield return new WaitForSeconds(bullet.GetComponent<Bullet>().FiringPeriod);
        }

    }

    void SelectBullet()
    {
        //press 1 to select next bullet
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            currentProjectile++;
            if(currentProjectile >= projectile.Length){
                currentProjectile = 0;
            }
        }
        //press 2 to select previous bullet
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            currentProjectile--;
            if(currentProjectile < 0){
                currentProjectile = projectile.Length - 1;
            }
        }

        bullet = projectile[currentProjectile];
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if(!other.gameObject.GetComponent<Enemy>()) return;

        DamageDealer damage = other.gameObject.GetComponent<DamageDealer>();
        if(damage){
            PlayerHP -= damage.getDamage();

            if(PlayerHP <= 0){
                GetComponent<CircleCollider2D>().enabled = false;
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
