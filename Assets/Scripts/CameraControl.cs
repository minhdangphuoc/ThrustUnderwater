using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera myCamera;
    CinemachineBrain myBrain;

    // Start is called before the first frame update
    void Start()
    {
        myBrain = GetComponent<CinemachineBrain>();
        myCamera = GetComponent<CinemachineVirtualCamera>();
        myCamera.enabled = true;
        myBrain.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
