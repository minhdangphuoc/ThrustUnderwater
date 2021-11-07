using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    Vector2 playerDirection, playerPosition;
    Vector2 mousePosition;
    bool playerIsMoving;
    bool haveCollider;

    public float grappleDistance = 1000f;

    Camera myCam;


    void Start()
    {
        myCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    
    void Update()
    {
        

        //Shoot
        if(Input.GetMouseButtonUp(0))
        {
            //get player direction 
            playerDirection = GetComponent<PlayerMovement>().shootDirection();
            RaycastHit2D rcHit = ShootToCursorPos();
            if (rcHit.collider != null)
            {
                haveCollider = !haveCollider;
                GetComponent<PlayerMovement>().GrappleToPoint(rcHit.collider.transform.position);
            }
        }
        
    }

    /// <summary>
    /// Shoot using mouse position
    /// </summary>
    RaycastHit2D ShootToCursorPos()
    {
        //Vector2 mousePosition = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height / 2);
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerPosition = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D rcHit = Physics2D.Raycast(playerPosition, (mousePosition - playerPosition).normalized, Mathf.Infinity, LayerMask.GetMask("Dashable"));
        return rcHit;
    }
}
