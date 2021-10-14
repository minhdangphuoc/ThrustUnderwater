using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera mainCamera;

    public CinemachineVirtualCamera subCamera;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = mainCamera.GetComponent<CinemachineVirtualCamera>();
        subCamera = subCamera.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Camera Change Zone"){
            ChangeToSubCam();
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Camera Change Zone"){
            ChangeToMainCam();
        }
    }

    void ChangeToSubCam()
    {
        mainCamera.Priority = subCamera.Priority - 1;
    }

    void ChangeToMainCam()
    {
        mainCamera.Priority = subCamera.Priority;
    }
}
