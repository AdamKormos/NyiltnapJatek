using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl05Enemy : MonoBehaviour
{
    [SerializeField] public byte health = 2;
    [SerializeField] public int scoreReward = 100;
    private byte maxHealth;

    private void Start()
    {
        maxHealth = health;
    }

    private void OnBecameVisible()
    {
        health = maxHealth;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            health--;
            Destroy(collision.gameObject);
            if (health == 0) Score.OnEnemyKilled(this);
        }
    }
}
