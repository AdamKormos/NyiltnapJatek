using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used on Lvl05 for spaceship bullets.
/// </summary>
public class Bullet : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, 0.1f);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
