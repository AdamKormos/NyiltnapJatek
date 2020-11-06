using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNS = GameNS;
using static UnityEngine.ParticleSystem;

public class Lvl05Server : MonoBehaviour
{
    [SerializeField] public byte maxHealth = 20;
    public static byte health;
    ParticleSystem particleSystem = default;
    Vector3 checkpointPosition;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        health = maxHealth;
        particleSystem.Stop();    
    }

    private void Update()
    {
        if (quizCollider.quizActive) checkpointPosition = transform.position;
        else if (Player.respawnedAtCheckpoint)
        {
            transform.position = checkpointPosition;
            health = maxHealth;
            GameNS::StaticData.gameUI.leftTopSlider.value = health;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Lvl05Enemy>() || collision.gameObject.GetComponent<EnemyBullet>())
        {
            OnDamageTaken(collision);
        }
    }

    private void OnDamageTaken(Collision2D enemyCol)
    {
        ShapeModule s = particleSystem.shape;
        s.position = new Vector3((enemyCol.transform.position.x - transform.position.x) / transform.lossyScale.x, s.position.y, s.position.z);
        particleSystem.Play();

        enemyCol.gameObject.GetComponent<Collider2D>().enabled = false;
        enemyCol.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        health--;
        GameNS::StaticData.gameUI.leftTopSlider.value--;
    }
}
