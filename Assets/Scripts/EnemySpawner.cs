using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public GameObject[] enemy;

    public int maxSpawnCount = 10;

    public float spawnRate = 2f;

    public float spawnDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator spawn()
    {
        while (true)
        {
            int point = 0;
            do { point = Random.Range(0, spawnPoints.Length); }
            while (Vector2.Distance(spawnPoints[point].transform.position, GameObject.FindWithTag("Player").transform.position) < spawnDistance);
            int enemyPF = Random.Range(0, enemy.Length);

            var enemies = FindObjectsOfType<Enemy>();
            Debug.Log(enemies.Length);

            if (enemies.Length < maxSpawnCount)
            {
                GameObject bornEnemy = Instantiate(enemy[enemyPF], spawnPoints[point].transform.position, transform.rotation);
                bornEnemy.transform.parent = gameObject.transform;
            }
            else
            {
                foreach (var obj in enemies)
                {
                    if (Vector2.Distance(obj.transform.position, GameObject.FindWithTag("Player").transform.position) > spawnDistance * 2)
                    {
                        Destroy(obj.gameObject);
                        break;
                    }
                }
            }


            yield return new WaitForSeconds(spawnRate);
        }
    }
}
