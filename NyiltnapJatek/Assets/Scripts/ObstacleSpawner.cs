using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The spawner class responsible for spawning objects.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject obstacleToSpawn = default;
    [SerializeField] protected float spawnCooldown = 20f;

    private void Start()
    {
        obstacleToSpawn.SetActive(false);
        StartCoroutine(Spawn());
    }

    /// <summary>
    /// Performs a default spawn behaviour on the gameobject passed in. Simply instantiates, then sets the object active. The rest of the desired behaviour must be done on
    /// one of the gameobject's scripts. Loops through this event each spawnCooldown seconds.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Spawn()
    {
        while (true)
        {
            GameObject obstacle = Instantiate(obstacleToSpawn, transform.position, obstacleToSpawn.transform.rotation, transform.parent);
            obstacle.SetActive(true);
            yield return new WaitForSeconds(spawnCooldown);
        }
    }
}
