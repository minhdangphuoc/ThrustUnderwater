using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    Vector2 targetLocation;
    Vector2 playerDirection;
    //Vector2 playerLocation;

    public float cameraOffset = 5f;
    public float cameraMoveTime = 2f;
    Vector2 smoothVelocity;

    void Start()
    {
        
    }

    void Update()
    {
        playerDirection = GetComponentInParent<PlayerMovement>().moveDirection.normalized;
        //playerLocation = GetComponentInParent<Transform>().position;
        targetLocation = playerDirection * cameraOffset;
        transform.localPosition = Vector2.SmoothDamp(transform.localPosition, targetLocation, ref smoothVelocity, cameraMoveTime);
    }
}
