using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    Vector2 playerDirection;
    Vector2 playerMovement;
    float playerSpeed;
    float playerAngle;
    float spriteAngle;

    Quaternion bodyRotation;
    float headRotation;
    Quaternion lowerBodyRotation;
    public float maxCloakTurnSpeed;

    public float turnSpeed;
    public float maxSpeed;
    public float cloakTurnSpeed;
    float smoothVelocity;
    float smoothVelocity2;

    float lastAngle;
    float angleChange;
    float lookDirection;
    float cloakAngle = 180f;

    void Start()
    {

    }

    void Update()
    {
        playerDirection = GetComponentInParent<PlayerMovement>().moveDirection;
        playerMovement = GetComponentInParent<PlayerMovement>().currentMovement;
        playerSpeed = playerMovement.magnitude;
        
        lastAngle = playerAngle;
        playerAngle = Vector2.SignedAngle(Vector2.up, playerMovement);
        spriteAngle = Mathf.SmoothDampAngle(spriteAngle, playerAngle, ref smoothVelocity, turnSpeed);
        //angleChange = Mathf.Clamp(Vector2.SignedAngle(playerDirection, playerMovement), -120, 120);
        angleChange = Mathf.Clamp(Mathf.DeltaAngle(playerAngle, spriteAngle), -90, 90);
        cloakAngle = Mathf.SmoothDampAngle(cloakAngle, 180 + angleChange , ref smoothVelocity2, cloakTurnSpeed);

        //headRotation = 
        if (playerMovement != Vector2.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, spriteAngle)), maxSpeed * Time.deltaTime);
            lowerBodyRotation = Quaternion.RotateTowards(lowerBodyRotation, Quaternion.Euler(new Vector3(0, 0, cloakAngle * Mathf.Sign(-lookDirection))), (maxCloakTurnSpeed * Time.deltaTime));
            GameObject.Find("BoneCloakMiddleUpper").transform.localRotation = lowerBodyRotation;
        }
        float angle = 0f;
        Vector3 angleAxis = Vector3.zero;
        (transform.rotation*Quaternion.Inverse(Quaternion.Euler(0, 0, 0))).ToAngleAxis(out angle, out angleAxis);
        if(Vector3.Angle(Vector3.forward, angleAxis) > 90f)
        {
            angle = -angle;
        }
        lookDirection = Mathf.DeltaAngle(0f, angle);

        //angleChange = transform.localScale.x != Mathf.Sign(-lookDirection) ? angle : angleChange;

        transform.localScale = new Vector3(Mathf.Sign(-lookDirection), 1, 1);
    }
    void OnGUI()
    {
        GUI.Label(new Rect(25, 70, 200, 40), "angle: " + angleChange);
    }
}
