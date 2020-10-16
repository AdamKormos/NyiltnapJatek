using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, 0.05f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Lvl05Enemy>())
        {
            Score.OnEnemyKilled(collision.gameObject.GetComponent<Lvl05Enemy>());
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
