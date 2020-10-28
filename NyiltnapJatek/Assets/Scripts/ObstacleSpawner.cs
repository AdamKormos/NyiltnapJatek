using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject obstacleToSpawn = default;
    [SerializeField] protected float spawnCooldown = 20f;

    private void Start()
    {
        obstacleToSpawn.SetActive(false);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            GameObject obstacle = Instantiate(obstacleToSpawn, transform.position, obstacleToSpawn.transform.rotation, transform.parent);
            obstacle.SetActive(true);
            yield return new WaitForSeconds(spawnCooldown);
        }
    }
}
