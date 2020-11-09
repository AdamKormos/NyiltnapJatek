using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNS = GameNS;
using static UnityEngine.ParticleSystem;

public class Lvl05Server : MonoBehaviour
{
    [SerializeField] public byte maxHealth = 20;
    public static byte health, s_maxHealth;
    ParticleSystem particleSystem = default;
    Vector3 checkpointPosition;
    public static bool dead = false;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        s_maxHealth = maxHealth;
        health = maxHealth;
        GameNS::StaticData.gameUI.leftTopSlider.maxValue = maxHealth;
        GameNS::StaticData.gameUI.leftTopSlider.value = GameNS::StaticData.gameUI.leftTopSlider.maxValue;
        particleSystem.Stop();    
    }

    private void Update()
    {
        if (quizCollider.quizActive) checkpointPosition = transform.position;

        Debug.Log(health);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canTakeDmg && (collision.gameObject.GetComponent<Lvl05Enemy>() || collision.gameObject.GetComponent<EnemyBullet>()))
        {
            StartCoroutine(OnDamageTaken(collision));
        }
    }

    bool canTakeDmg = true;


    private IEnumerator OnDamageTaken(Collision2D enemyCol)
    {
        ShapeModule s = particleSystem.shape;
        s.position = new Vector3((enemyCol.transform.position.x - transform.position.x) / transform.lossyScale.x, s.position.y, s.position.z);
        particleSystem.Play();

        enemyCol.gameObject.GetComponent<Collider2D>().enabled = false;
        enemyCol.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        health--;
        Debug.Log("Dmg taken");
        GameNS::StaticData.gameUI.leftTopSlider.value--;
        if (health == 0)
        {
            dead = true;
            Debug.Log("MAX: " + maxHealth);
            health = maxHealth;
            GameNS::StaticData.gameUI.leftTopSlider.value = health;
            transform.position = checkpointPosition;
        }

        canTakeDmg = false;
        yield return new WaitForSeconds(1f);
        canTakeDmg = true;
    }
}
