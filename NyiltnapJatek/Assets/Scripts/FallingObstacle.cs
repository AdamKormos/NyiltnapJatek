using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacle
{
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
