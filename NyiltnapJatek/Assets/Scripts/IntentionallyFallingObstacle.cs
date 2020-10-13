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
            StartCoroutine(ObstacleFall());
        }
    }

    public IEnumerator ObstacleFall()
    {
        for (int i = 0; i < 200 * (objectFallStrength / 0.01f); i++)
        {
            transform.position -= new Vector3(0, objectFallStrength);
            yield return new WaitForEndOfFrame();
        }

        Destroy(this.gameObject);
    }
}
