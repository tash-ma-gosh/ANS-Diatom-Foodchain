using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Predator;
    public float spawnInterval = 5f;
    public Transform[] spawnPoints;

    private float timeSinceLastSpawn = 0f;

    public float minDistance = 2f;
    
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
        // Bounds spawnBounds = CalculateSpawnBounds();
        // Vector3 spawnPosition = GetRandomSpawnPosition(spawnBounds);
        //Instantiate(Predator, spawnPosition, Quaternion.identity);

        //  bool isValidPosition = CheckMinimumDistance(spawnPosition);

        // Retry if the position is invalid
        // while (!isValidPosition)
        // {
        //     spawnPosition = GetRandomSpawnPosition(spawnBounds);
        //     isValidPosition = CheckMinimumDistance(spawnPosition);
        // }

        //GameObject newPrefab = Instantiate(Predator, spawnPosition, Quaternion.identity);
        //newPrefab.GetComponent<PrefabChaseBehavior>().targetObject = targetObject;

        Vector3 randomRange = new Vector3 (Random.Range(-70,70),Random.Range(-40,40),0);
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(Predator, randomRange, Quaternion.identity);
    }

    

    private Bounds CalculateSpawnBounds()
    {
        Renderer[] sceneRenderers = FindObjectsOfType<Renderer>();

        Bounds spawnBounds = new Bounds();

        foreach (Renderer renderer in sceneRenderers)
        {
            spawnBounds.Encapsulate(renderer.bounds);
        }

        return spawnBounds;
    }

    private Vector3 GetRandomSpawnPosition(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        Vector3 spawnPosition = new Vector3(x, 0f, z);
        return spawnPosition;
    }

     private bool CheckMinimumDistance(Vector3 position)
    {
        GameObject[] existingPrefabs = GameObject.FindGameObjectsWithTag("predatorPrefab");

        foreach (GameObject existingPrefab in existingPrefabs)
        {
            float distance = Vector3.Distance(existingPrefab.transform.position, position);
            if (distance < minDistance)
            {
                return false; // Minimum distance constraint violated
            }
        }

        return true; // Position satisfies the minimum distance constraint
    }

}
