using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlobEnemyControl : MonoBehaviour
{

    private GameObject blob, targetPoint;
    float timer;
    public float spawnTime = 2f;
    bool shouldSpawn;
    public float speed = 2f;

    void Start()
    {
        blob = transform.Find("Blob").gameObject;
        targetPoint = transform.Find("TargetPoint").gameObject;
        //targetPoint = GameObject.Find("TargetPoint");
    }

    void Update()
    {
        blob.transform.Rotate(new Vector3 (0, 0, 0.5f));
        if (shouldSpawn)
        {
            blob.transform.position = transform.position;
            shouldSpawn = false;
            timer = spawnTime;
        } else
        {
            if (blob.transform.position != targetPoint.transform.position)
            {
                blob.transform.position = Vector3.MoveTowards(blob.transform.position, targetPoint.transform.position, speed * Time.deltaTime);
            }
            if (timer <= 0)
            {
                shouldSpawn = true;
            } else
            {
                timer -= Time.deltaTime;
            }
        }
    }
}
