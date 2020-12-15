using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used on Lvl05 enemy spaceships. Potentially could be removed as its movement can be replaced with a ConstantMovement script and a tag could be used to identify it?
/// TODO: Investigate the thing described here ^
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0f, 0.1f);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
