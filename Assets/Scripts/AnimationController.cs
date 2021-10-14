using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public bool facingLeft = true;

    Vector2 playerDirection;
    Vector2 playerMovement;
    float playerAngle;
    float spriteAngle;

    Quaternion bodyRotation;
    float headRotation;
    float lowerBodyRotation;

    public float turnSpeed;
    public float maxSpeed;
    float smoothVelocity;

    float angleDiff;
    float lookDirection;

    void Start()
    {

    }

    void Update()
    {
        playerDirection = GetComponentInParent<PlayerMovement>().moveDirection;
        playerMovement = GetComponentInParent<PlayerMovement>().currentMovement;

        playerAngle = Vector2.SignedAngle(Vector2.up, playerMovement);
        spriteAngle = Mathf.SmoothDampAngle(spriteAngle, playerAngle, ref smoothVelocity, turnSpeed);

        //headRotation = 
        if (playerMovement != Vector2.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, spriteAngle)), maxSpeed * Time.deltaTime);
        }
        
        float angle = 0f;
        Vector3 angleAxis = Vector3.zero;
        (transform.rotation*Quaternion.Inverse(Quaternion.Euler(0, 0, 0))).ToAngleAxis(out angle, out angleAxis);
        if(Vector3.Angle(Vector3.forward, angleAxis) > 90f)
        {
            angle = -angle;
        }
        lookDirection = Mathf.DeltaAngle(0f, angle);
        
        transform.localScale = new Vector3(Mathf.Sign(-lookDirection), 1, 1);
    }
    void OnGUI()
    {
        GUI.Label(new Rect(25, 70, 200, 40), "angle: " + lookDirection);
    }
}
