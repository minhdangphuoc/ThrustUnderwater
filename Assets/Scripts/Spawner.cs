using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] spawnObject;

    public int maxSpawnAmount = 5;

    public float minDisBetweenObjs = 3f;

    public float spawnRate = 2f;

    int currentAmount = 0;

    List<GameObject> totalAmount;

    BoxCollider2D spawnArea;

    // Start is called before the first frame update
    void Start()
    {
        spawnArea = gameObject.GetComponent<BoxCollider2D>();
        totalAmount = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnObject()
    {
        while(currentAmount < maxSpawnAmount)
        {

            //generate random position
            Vector2 SpawnPosition = new Vector2(Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y));

            //Not spawn near player
            if (Vector2.Distance(SpawnPosition, GameObject.FindWithTag("Player").transform.position) < minDisBetweenObjs*2) continue;

            //spawn if not too close
            if(isNotClose(SpawnPosition)) Instantiate(spawnObject[Random.Range(0, spawnObject.Length)], SpawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnRate);
        }
    }

    /// <summary>
    /// Check if there is an object near the position
    /// </summary>
    /// <param name="PosToCheck">position to check</param>
    /// <returns></returns>
    bool isNotClose(Vector2 PosToCheck)
    {
        if (totalAmount.Count == 0) return true;

        foreach(var obj in totalAmount)
        {
            if (Vector2.Distance(obj.transform.position, PosToCheck) < minDisBetweenObjs) return false;
        }
        return true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player") StartCoroutine(SpawnObject());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>()) {
            totalAmount.Add(collision.gameObject);
            currentAmount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            totalAmount.Remove(collision.gameObject);
            currentAmount--;
        }
    }
}
