using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNew : MonoBehaviour
{
    // Public Attribute
    public GameObject[] Orbs;
    private int MAX_RANDOM_ORBS = 10;
    private string orb_name = "";

    Vector2 moveDirection;
    Vector2 currentMovement;

    //Larger = more
    public float accelerationTime = 0.2f;
    public float decelerationTime = 0.3f;
    //Larger = less
    public float slide = 6f;
    public float floatiness = 20f;

    public float speed = 6f;
    public float dashPower = 10f;

    public float dashCooldown = 1f;
    private float dashCooldownTimer = 0f;
    
    private bool haveMovementInput;
    private Vector2 velocity;

    void Start()
    {
        for (int i = 0; i < MAX_RANDOM_ORBS; i++)
        {
            int randomIndex = Random.Range(0, Orbs.Length);
            GameObject instantiatedObject = Instantiate(Orbs[randomIndex], new Vector3(Random.Range(-10, 9) + 0.5f, Random.Range(-4, 3) + 0.5f, 0), Quaternion.identity) as GameObject;
        }
    }

    void Update()
    {
        moveDirection.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection.Normalize();
        haveMovementInput = (Input.GetAxisRaw("Horizontal") != 0f) || (Input.GetAxisRaw("Vertical") != 0f);

        if (dashCooldownTimer <= 0 && Input.GetButtonDown("Dash") && haveMovementInput)
        {
            currentMovement = (currentMovement / 2) + moveDirection * dashPower;
            dashCooldownTimer = dashCooldown;
        } else if (dashCooldownTimer <= 0 && Input.GetButtonDown("Dash") && currentMovement.magnitude > 0.1f)
        {
            currentMovement = (currentMovement / 2) +  currentMovement.normalized * dashPower;
            dashCooldownTimer = dashCooldown;
        }
        if (haveMovementInput)
        {
            currentMovement = Vector2.SmoothDamp(currentMovement, moveDirection, ref velocity, accelerationTime + (currentMovement.sqrMagnitude / floatiness));
        } else
        {
            currentMovement = Vector2.SmoothDamp(currentMovement, Vector2.zero, ref velocity, decelerationTime + (currentMovement.sqrMagnitude / slide));
        }
        dashCooldownTimer -= Time.deltaTime;

        transform.Translate(currentMovement * speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "orb")
        {
            orb_name = other.gameObject.name;
            Destroy(other.gameObject);
        } 
        
    }

    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 200, 40), "dash cd: " + dashCooldownTimer);
        GUI.Label(new Rect(25, 40, 200, 40), "dir: " + moveDirection);
        GUI.Label(new Rect(25, 55, 200, 40), "speed: " + currentMovement.magnitude);
        GUI.Label(new Rect(25, 70, 200, 40), "Orb: " + orb_name);
    }
}
