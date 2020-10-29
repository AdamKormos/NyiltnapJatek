using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl05Enemy : MonoBehaviour
{
    [SerializeField] public byte health = 2;
    [SerializeField] public int scoreReward = 100;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            health--;
            Destroy(collision.gameObject);
            if(health == 0) Score.OnEnemyKilled(this);
        }
    }
}
