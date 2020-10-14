using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntentionallyFallingObstacle : MonoBehaviour
{
    [SerializeField] float objectFallStrength = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerLvl02MatekFizika>())
        {
            StartCoroutine(FallingObstacle.ObstacleFall(this.transform, objectFallStrength));
        }
    }
}
