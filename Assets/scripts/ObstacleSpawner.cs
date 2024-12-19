using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float spawnRate = 1.4f;
    public Vector2 xPositionRange = new Vector2(-5.6f, 5.9f); 
    public float yPosition = 11; 
    public float zPosition = 17.6f; 

    private Transform rampTransform;

    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        if (spawnRate > 0.2f)
        {
            spawnRate = spawnRate - 0.08f;
        }
        else if (spawnRate > 0.05)
        {
            spawnRate = spawnRate - 0.0001f;
        }
       
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        
        float randomX = Random.Range(xPositionRange.x, xPositionRange.y);
        
        Vector3 spawnPosition = new Vector3(randomX, yPosition, zPosition);
        
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, obstaclePrefab.transform.rotation);
        
        Rigidbody obstacleRb = obstacle.GetComponent<Rigidbody>();
        if (obstacleRb == null)
        {
            obstacleRb = obstacle.AddComponent<Rigidbody>();
        }

        obstacleRb.useGravity = true;
        obstacleRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
    }
}
