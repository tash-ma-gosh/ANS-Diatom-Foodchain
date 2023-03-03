using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Predator;
    public float spawnInterval = 2f;
    public Transform[] spawnPoints;

    private float timeSinceLastSpawn = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        
        Vector3 randomRange = new Vector3 (Random.Range(-70,70),Random.Range(-40,40),0);
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(Predator, randomRange, Quaternion.identity);
    }
}
