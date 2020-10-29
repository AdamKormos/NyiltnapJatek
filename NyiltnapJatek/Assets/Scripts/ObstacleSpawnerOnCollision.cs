using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawnerOnCollision : ObstacleSpawner
{
    bool started = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!started && collision.gameObject.GetComponent<Player>()) StartCoroutine(Spawn());
    }

    private void Start()
    {
        obstacleToSpawn.SetActive(false);
    }

    IEnumerator Spawn()
    {
        started = true;
        while (true)
        {
            GameObject obstacle = Instantiate(obstacleToSpawn, transform.position, obstacleToSpawn.transform.rotation, transform.parent);
            obstacle.SetActive(true);
            yield return new WaitForSeconds(spawnCooldown);
        }
    }
}
