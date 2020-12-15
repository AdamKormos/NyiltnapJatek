using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The spawner class responsible for spawning objects whenever the player enters its spawn trigger zone.
/// </summary>
public class ObstacleSpawnerOnCollision : ObstacleSpawner
{
    bool isPlayerInSpawnTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayerInSpawnTrigger && collision.gameObject.GetComponent<Player>())
        {
            isPlayerInSpawnTrigger = true;
            StartCoroutine(Spawn());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPlayerInSpawnTrigger && collision.gameObject.GetComponent<Player>())
        {
            isPlayerInSpawnTrigger = false;
        }
    }

    private void Start()
    {
        obstacleToSpawn.SetActive(false);
    }

    /// <summary>
    /// Performs a default spawn behaviour on the gameobject passed in. Simply instantiates, then sets the object active. The rest of the desired behaviour must be done on
    /// one of the gameobject's scripts. Loops through this event each spawnCooldown seconds as long as the player is in the spawn trigger area.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Spawn()
    {
        while (isPlayerInSpawnTrigger)
        {
            GameObject obstacle = Instantiate(obstacleToSpawn, transform.position, obstacleToSpawn.transform.rotation, transform.parent);
            obstacle.SetActive(true);
            yield return new WaitForSeconds(spawnCooldown);
        }
    }
}
