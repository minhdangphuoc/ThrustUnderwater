using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNew : MonoBehaviour
{

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

    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 200, 40), "dash cd: " + dashCooldownTimer);
        GUI.Label(new Rect(25, 40, 200, 40), "dir: " + moveDirection);
        GUI.Label(new Rect(25, 55, 200, 40), "speed: " + currentMovement.magnitude);
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide");
        Destroy(other.gameObject);
    }
}
