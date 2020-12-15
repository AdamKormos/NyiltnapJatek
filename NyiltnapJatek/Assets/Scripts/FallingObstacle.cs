using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Truly, a static wannabe class with a single method. Originally this was the class for falling obstacles but as new ideas appeared, it became obsolate.
/// TODO: Check if this is by any chance on any objects inside Unity. If not, make this class static.
/// </summary>
public class FallingObstacle
{
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
            if(!quizCollider.quizActive) obstacleT.position -= new Vector3(0, objectFallStrength);
            yield return new WaitForEndOfFrame();
        }

        GameObject.Destroy(obstacleT.gameObject);
    }
}
