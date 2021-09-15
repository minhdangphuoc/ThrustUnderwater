using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //private bool leftKey = false;
    //private bool rightKey = false;
    public float rotationSpeed;
    public float movementSpeed;
    public float playerAngle;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerAngle -= Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
        rb.AddRelativeForce(new Vector2(0f, (Input.GetAxis("Vertical") * movementSpeed * Time.fixedDeltaTime)));
        rb.MoveRotation(playerAngle);


    }
}
