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
    
    private Vector2 velocity;

    void Start()
    {
        //moveDirection.Set(1, 0);
    }

    void Update()
    {
        moveDirection.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection.Normalize();
        if (Input.GetAxisRaw("Horizontal") == 0f && Input.GetAxisRaw("Vertical") == 0f)
        {
            currentMovement = Vector2.SmoothDamp(currentMovement, Vector2.zero, ref velocity, decelerationTime + (currentMovement.sqrMagnitude / slide));
        } else
        {
            currentMovement = Vector2.SmoothDamp(currentMovement, moveDirection, ref velocity, accelerationTime + (currentMovement.sqrMagnitude / floatiness));
        }
        transform.Translate(currentMovement * speed * Time.deltaTime);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 200, 40), "speed: " + speed);
        GUI.Label(new Rect(25, 40, 200, 40), "dir: " + moveDirection);
        GUI.Label(new Rect(25, 55, 200, 40), "dir: " + currentMovement.sqrMagnitude);
    }
}
