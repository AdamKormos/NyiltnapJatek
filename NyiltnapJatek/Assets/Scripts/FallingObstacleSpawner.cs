using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The spawner class responsible for spawning objects that fall down at a constant speed.
/// TODO: Consider replacing the whole falling thing (classes for spawners)? As it could be done with ConstantMovement scripts and base spawner.
/// </summary>
public class FallingObstacleSpawner : ObstacleSpawner
{
    [SerializeField] protected float objectFallStrength = 0.05f;

    private void Start()
    {
        obstacleToSpawn.SetActive(false);
        StartCoroutine(Spawn());
    }

    /// <summary>
    /// Performs a default spawn behaviour on the gameobject passed in. Simply instantiates, sets the object active and makes it fall. 
    /// The rest of the desired behaviour must be done on one of the gameobject's scripts. Loops through this event each spawnCooldown seconds.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Spawn()
    {
        while(true)
        {
            GameObject obstacle = Instantiate(obstacleToSpawn, transform.position, obstacleToSpawn.transform.rotation);
            obstacle.SetActive(true);
            StartCoroutine(ObstacleFall(obstacle.transform, objectFallStrength));
            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    /// <summary>
    /// Method to make obstacles fall with a constant speed.
    /// </summary>
    /// <param name="obstacleT"></param>
    /// <param name="objectFallStrength"></param>
    /// <returns></returns>
    public static IEnumerator ObstacleFall(Transform obstacleT, float objectFallStrength)
    {
        float positionToDestroyFrom = Camera.main.transform.position.y - Camera.main.orthographicSize - (obstacleT.GetComponent<SpriteRenderer>().bounds.size.y / 2 * obstacleT.localScale.y);

        while (obstacleT.position.y > positionToDestroyFrom)
        {
            if (!quizCollider.quizActive) obstacleT.position -= new Vector3(0, objectFallStrength);
            yield return new WaitForEndOfFrame();
        }

        GameObject.Destroy(obstacleT.gameObject);
    }
}
