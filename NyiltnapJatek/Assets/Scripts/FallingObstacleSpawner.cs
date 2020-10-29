using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacleSpawner : ObstacleSpawner
{
    [SerializeField] protected float objectFallStrength = 0.05f;

    private void Start()
    {
        obstacleToSpawn.SetActive(false);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(true)
        {
            GameObject obstacle = Instantiate(obstacleToSpawn, transform.position, obstacleToSpawn.transform.rotation);
            StartCoroutine(FallingObstacle.ObstacleFall(obstacle.transform, objectFallStrength));
            yield return new WaitForSeconds(spawnCooldown);
        }
    }
}
