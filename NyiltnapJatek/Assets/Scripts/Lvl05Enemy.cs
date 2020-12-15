using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy class for Lvl05.
/// </summary>
public class Lvl05Enemy : MonoBehaviour
{
    [SerializeField] public byte health = 2;
    [SerializeField] public int scoreReward = 100;
    private byte maxHealth;

    private void Start()
    {
        maxHealth = health;
    }

    /// <summary>
    /// Does what it does because of checkpoint simulation, as now, if enemies appear on the screen, they have full health. If they were to be killed, enemies wouldn't
    /// appear again if they were killed but the player spawned at a checkpoint.
    /// </summary>
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
