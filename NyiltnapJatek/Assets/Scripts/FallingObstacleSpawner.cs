using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject obstacleToSpawn = default;
    [SerializeField] float spawnCooldown = 20f;
    [SerializeField] float objectFallStrength = 0.05f;

    private void Start()
    {
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
