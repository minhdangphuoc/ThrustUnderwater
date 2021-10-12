using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //public float rotationSpeed;
    //public float playerAngle;
    public float horizontalSpeed;
    public float verticalSpeed;
    public float additionalSpeed;

    float v;
    //private float turnAmount;

    private Rigidbody2D rb;

    private bool playerMoving;
    private bool haveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerMoving = false;
        haveInput = false;
    }

    void FixedUpdate()
    {
        haveInput = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + (Mathf.Abs(Input.GetAxisRaw("Vertical")))) != 0;
        
        playerMoving = haveInput;
        if (playerMoving) {
            rb.gravityScale = 0;
        } else {
            //rb.gravityScale = 1;
        }

        rb.AddForce(new Vector2((Input.GetAxis("Horizontal") * (horizontalSpeed + additionalSpeed) * Time.fixedDeltaTime), (Input.GetAxis("Vertical") * (verticalSpeed + additionalSpeed) * Time.fixedDeltaTime)));

        v = rb.velocity.magnitude;
    }
    
}
