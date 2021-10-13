using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraSize : MonoBehaviour
{

    public Camera MyCamera;
    // Start is called before the first frame update
    void Start()
    {
        MyCamera.enabled = true;
        MyCamera.orthographicSize = 4f;
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("MainCamera").transform.position = gameObject.transform.position;
    }
}
