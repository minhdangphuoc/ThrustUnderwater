using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    Vector2 playerPosition;
    Vector2 mousePosition;
    bool playerIsMoving;

    public float grappleDistance = 10f;
    public float grappleSpeed = 5f;
    Vector3 grapplePoint;
    Vector3 hookPoint;

    Camera myCam;

    LineRenderer lineRenderer;
    bool isRendering;
    bool haveGrapplePoint;
    bool willDash;
    bool canGrapple;
    bool isReturning;

    void Start()
    {
        myCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.positionCount = 2;
        canGrapple = true;
    }

    
    void Update()
    {
        //Shoot
        if(Input.GetMouseButtonUp(0) && canGrapple)
        {
            RaycastHit2D rcHit = ShootToCursorPos();
            if (rcHit.collider != null && Vector3.Distance(rcHit.collider.transform.position, transform.position) < grappleDistance + 1.5f)
            {
                grapplePoint = rcHit.collider.transform.position;
                willDash = true;
                //GetComponent<PlayerMovement>().GrappleToPoint(rcHit.collider.transform.position);
            } else
            {
                Vector3 shootPoint = (mousePosition - playerPosition).normalized * grappleDistance;
                grapplePoint = transform.position + shootPoint;
                willDash = false;
            }
            haveGrapplePoint = true;
            hookPoint = transform.position;
            canGrapple = false;
            isRendering = true;
            isReturning = false;
            lineRenderer.enabled = true;
        }
        if (haveGrapplePoint)
        {
            if (hookPoint != grapplePoint && !isReturning)
            {
                hookPoint = Vector3.MoveTowards(hookPoint, grapplePoint, grappleSpeed * Time.deltaTime * 10);
            } else if (Vector3.Distance(hookPoint, transform.position) > 1 && isReturning)
            {
                hookPoint = Vector3.MoveTowards(hookPoint, transform.position, grappleSpeed * 2 * Time.deltaTime * 10);
            } else
            {
                if (isReturning)
                {
                    EndGrapple();
                } else if (willDash)
                {
                    GetComponent<PlayerMovement>().GrappleToPoint(grapplePoint);
                } else
                {
                    isReturning = true;
                }
            }
        }
        if (isRendering)
        {
            var points = new Vector3[2];
            points[0] = transform.position;
            points[1] = hookPoint;
            lineRenderer.SetPositions(points);
        }
    }
    public void EndGrapple()
    {
        canGrapple = true;
        isRendering = false;
        haveGrapplePoint = false;
        lineRenderer.enabled = false;
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
