using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl05Enemy : MonoBehaviour
{
    [SerializeField] public byte health = 2;
    [SerializeField] public int scoreReward = 100;
    bool isAttacking = false;

    private void Update()
    {
        if(isAttacking)
        {
            // ...
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            health--;
            Destroy(collision.gameObject);
            if(health == 0) Score.OnEnemyKilled(this);
        }
    }

    private void OnBecameVisible()
    {
        isAttacking = true;
    }

    private void OnBecameInvisible()
    {
        isAttacking = false;
    }
}
