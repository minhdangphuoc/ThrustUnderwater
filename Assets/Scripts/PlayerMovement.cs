using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Public Attribute
    public GameObject[] Orbs;
    private int MAX_RANDOM_ORBS = 10;
    private string orb_name = "";
    
    

    public Vector2 moveDirection;
    public Vector2 currentMovement;
    private Vector2 dashDirection;
    private Vector2 dashTarget;

    //Larger = more
    public float accelerationTime = 0.2f;
    public float decelerationTime = 0.3f;
    //Larger = less
    public float slide = 6f;
    public float floatiness = 20f;

    public float speed = 6f;
    public float dashPower = 20f;
    public float dashSpeed = 2f;
    public float dashEndSpeedMultiplier = 2f;
    private bool dashEndSpeedBurst;

    public float dashCooldown = 1f;
    private float dashCooldownTimer = 0f;
    
    private bool haveMovementInput;
    public bool haveControl = true;

    private bool hasKnockbackActive;
    public float knockbackTime = 2f;
    private bool isDashing;
    private bool isGrappling;

    private Vector2 velocity;
    
    
    void Start()
    {
        /*for (int i = 0; i < MAX_RANDOM_ORBS; i++)
        {
            int randomIndex = Random.Range(0, Orbs.Length);
            GameObject instantiatedObject = Instantiate(Orbs[randomIndex], new Vector3(Random.Range(-10, 9) + 0.5f, Random.Range(-4, 3) + 0.5f, 0), Quaternion.identity) as GameObject;
        }*/
    }

    void Update()
    {
        //if (isDashing) haveControl = false;
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
        
        moveDirection.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection.Normalize();
        haveMovementInput = (Input.GetAxisRaw("Horizontal") != 0f) || (Input.GetAxisRaw("Vertical") != 0f);
        
        //if (Input.GetAxis("Horizontal") < 0) front_light_change_dir(90.0f);
        //else if (Input.GetAxis("Horizontal") > 0) front_light_change_dir(-90f);
        if (haveControl && !isDashing && CheckDashInput())
        {
            dashTarget = new Vector2(transform.position.x, transform.position.y) + (dashDirection * dashPower);
            StartDash(dashTarget, false);
            dashCooldownTimer = dashCooldown;
        }
        if (isDashing) UpdateDash();
        if (hasKnockbackActive) UpdateKnockback();
        if (haveMovementInput && haveControl)
        {
            currentMovement = Vector2.SmoothDamp(currentMovement, moveDirection, ref velocity, accelerationTime + (currentMovement.sqrMagnitude / floatiness));
        } else if (haveControl)
        {
            currentMovement = Vector2.SmoothDamp(currentMovement, Vector2.zero, ref velocity, decelerationTime + (currentMovement.sqrMagnitude / slide));
        }
        if (currentMovement.sqrMagnitude > 0.01)
        {
            front_light_change_dir(Vector2.SignedAngle(Vector2.up, currentMovement));
        }
        dashCooldownTimer -= Time.deltaTime;
        if (haveControl) transform.Translate(currentMovement * speed * Time.deltaTime);
    }
    
    private void front_light_change_dir(float tiltAngle){
        Quaternion target = Quaternion.Euler(0, 0, tiltAngle);
        Quaternion current = GameObject.Find("FrontLight").transform.rotation;
        GameObject.Find("FrontLight").transform.rotation = Quaternion.Slerp(current, target,  Time.deltaTime * 10f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "orb")
        {
            orb_name = other.gameObject.name;
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "Game Wall" && !isGrappling)
        {
            TakeDamage(true);
        }
    }

    private bool CheckDashInput()
    {
        if (dashCooldownTimer <= 0 && Input.GetButtonDown("Dash") && haveMovementInput)
        {
            //currentMovement = (currentMovement / 2) + moveDirection * dashPower;
            if (Mathf.Abs(Vector3.Angle(moveDirection, currentMovement)) <= 90f)
            {
                dashDirection = (currentMovement.normalized / 2) + (moveDirection / 2);
            } else
            {
                dashDirection = moveDirection / 1.5f;
            }
            return true;
        } else if (dashCooldownTimer <= 0 && Input.GetButtonDown("Dash") && currentMovement.magnitude > 0.1f)
        {
            //currentMovement = (currentMovement / 2) +  currentMovement.normalized * dashPower;
            dashDirection = currentMovement.normalized;
            return true;
        }
        return false;
    }

    public void StartDash(Vector2 location, bool isGrappleDash)
    {   
        dashTarget = location;
        if (isGrappleDash) dashDirection = dashTarget - new Vector2(transform.position.x, transform.position.y);
        moveDirection = dashDirection;
        currentMovement = dashDirection;
        //transform.position = location;
        dashEndSpeedBurst = !isGrappleDash;
        isDashing = true;
        haveControl = false;

        GetComponentInChildren<CameraFollowObject>().transform.position = dashTarget;
    }
    private void UpdateDash()
    {
        if (!transform.position.Equals(dashTarget))
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, (dashSpeed * (dashEndSpeedBurst ? 1 : 2)) * Time.deltaTime * 10);
        } else
        {
            EndDash();
        }
    }
    private void EndDash()
    {
        //transform.position = dashTarget;
        isDashing = false;
        isGrappling = false;
        haveControl = true;
        if (dashEndSpeedBurst)
        {
            currentMovement = dashDirection.normalized * dashEndSpeedMultiplier;
        } else
        {
            moveDirection = Vector2.zero;
            currentMovement = Vector2.zero;
        }
        GetComponent<GrapplingHook>().EndGrapple();
    }

    public void GrappleToPoint(Vector3 target)
    {
        isGrappling = true;
        StartDash(target, true);
    }

    public Vector2 shootDirection()
    {
        return moveDirection;
    }

    public void TakeDamage(bool hasKnockback)
    {
        EndDash();
        if (hasKnockback)
        {
            StartKnockback(FindKnockbackPoint(transform.position));
        }
        //if (!isDashing) SceneManager.LoadScene(0);
    }

    private void StartKnockback(Vector2 location)
    {
        dashTarget = location;
        dashDirection = (dashTarget - new Vector2(transform.position.x, transform.position.y)).normalized;
        moveDirection = dashDirection;
        currentMovement = dashDirection;
        haveControl = false;
        hasKnockbackActive = true;

        GetComponentInChildren<CameraFollowObject>().transform.position = dashTarget;
    }

    private void UpdateKnockback()
    {
        if (Vector2.Distance(transform.position, dashTarget) > 0.1f)
        {
            transform.position = Vector2.SmoothDamp(transform.position, dashTarget, ref velocity, knockbackTime);
        } else
        {
            EndKnockback();
        }
    }

    private void EndKnockback()
    {
        hasKnockbackActive = false;
        haveControl = true;
        moveDirection = Vector2.zero;
        currentMovement = Vector2.zero;
    }

    private Vector2 FindKnockbackPoint(Vector2 fromLocation)
    {
        Vector2 target = new Vector2();

        Collider2D[] colliders = GameObject.FindGameObjectWithTag("Safe Zone").GetComponents<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Vector2 checkedLocation = colliders[i].ClosestPoint(fromLocation);
            if (!checkedLocation.Equals(fromLocation))
            {
                if (i == 0 || (Vector2.Distance(fromLocation, checkedLocation) < Vector2.Distance(fromLocation, target)))
                {
                    target = checkedLocation;
                }
            }
        }

        return target;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(25, 25, 200, 40), "dash cd: " + dashCooldownTimer);
        GUI.Label(new Rect(25, 40, 200, 40), "dir: " + moveDirection);
        GUI.Label(new Rect(25, 55, 200, 40), "speed: " + currentMovement.magnitude);
        //GUI.Label(new Rect(25, 70, 200, 40), "Orb: " + orb_name);
    }
}
