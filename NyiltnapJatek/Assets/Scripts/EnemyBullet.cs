using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
