using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{

    Transform camTransform;
    Vector3 lastCamPosition;
    public float effectMultiplier = 0.2f;

    void Start()
    {
        camTransform = Camera.main.transform;
        lastCamPosition = camTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = camTransform.position - lastCamPosition;
        transform.Translate(new Vector3(-deltaMovement.x * effectMultiplier, 0, 0));
        lastCamPosition = camTransform.position;
    }
}
