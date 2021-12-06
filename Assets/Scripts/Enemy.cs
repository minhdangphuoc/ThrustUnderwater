using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// This game object's health
    /// </summary>
    public float HP = 100;

    public float moveSpeed = 5f;

    public float roamRadius = 20f;

    public float distanceToChasePlayer = 10f;

    public float moveAccuracy = 3f;

    public Animator animator;

    Vector2 startPosition, roamPosition;
    Vector2 currentPosition, previousPosition;

    enum State
    {
        Roam,
        ChasePlayer
    }

    State currentState;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        currentPosition = transform.position;
        previousPosition = transform.position;
        roamPosition = RoamPosition();
        currentState = State.Roam;

        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPosition == previousPosition)
            currentState = State.Roam;

        StatesMachine();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //get damage dealer
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        //if not a damage dealer then return
        if (!damageDealer) return;
        //Chase player if get hit
        currentState = State.ChasePlayer;
        //decrease HP
        HP -= damageDealer.getDamage();
        //destroy game object
        if (HP <= 0) Destroy(gameObject);
    }

    void StatesMachine()
    {
        switch (currentState)
        {
            case State.Roam:
                Roam();
                break;
            case State.ChasePlayer:
                StartCoroutine(ChasePlayer());
                break;
        }
    }

    void Roam()
    {
        animator.SetBool("Back", true);

        currentPosition = roamPosition;

        //Move to random position
        transform.position = Vector2.MoveTowards(transform.position, roamPosition, moveSpeed * Time.deltaTime);

        //if there are obstacles or reach destination then go to new position
        if (CheckCollisions(roamPosition - (Vector2)transform.position) || Vector2.Distance(transform.position, roamPosition) < moveAccuracy)
        {
            roamPosition = RoamPosition();
        }

        //if near player start chasing
        if (Vector2.Distance(transform.position, player.transform.position) < distanceToChasePlayer)
        {
            currentState = State.ChasePlayer;
        }
    }

    IEnumerator ChasePlayer()
    {
        animator.SetBool("Forward", true);

        //Move towards player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

        yield return new WaitForSeconds(moveAccuracy);

        //If out of radius go back to roaming
        if (Vector2.Distance(startPosition, player.transform.position) > roamRadius) currentState = State.Roam;
    }

    /// <summary>
    /// Generate a position to roam to
    /// </summary>
    /// <returns></returns>
    Vector2 RoamPosition()
    {
        return startPosition + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(0f, roamRadius);
    }

    bool CheckCollisions(Vector2 direction)
    {
        //Cast a ray to check for collisions
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, direction.normalized, moveAccuracy);
        foreach (var obj in hit)
        {
            if (obj.collider.tag != "EnemySpawner" && obj.collider.tag != "Camera Constraint" && obj.collider.tag != "Enemy")
            {
                return true;
            }
        }
        return false;
    }
}
