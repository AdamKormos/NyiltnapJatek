﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl05SpaceshipEnemy : MonoBehaviour
{
    [SerializeField] EnemyBullet bullet = default;
    [SerializeField] public byte health = 2;
    [SerializeField] public int scoreReward = 100;
    bool isOnScreen = false;
    float halfSpriteHeight = 0f;

    private void Start()
    {
        halfSpriteHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
        StartCoroutine(Shoot());
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;
    }

    IEnumerator Shoot()
    {
        while(isOnScreen)
        {
            GameObject b = Instantiate(bullet.gameObject, transform.position - new Vector3(0, halfSpriteHeight, 0), Quaternion.identity, transform.parent);
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
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