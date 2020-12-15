using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

/// <summary>
/// The class for the server on Lvl05, the object to protect from enemies.
/// </summary>
public class Lvl05Server : MonoBehaviour
{
    [SerializeField] public byte maxHealth = 20;
    public static byte health, s_maxHealth;
    new ParticleSystem particleSystem = default;
    Vector3 checkpointPosition; // TODO: Find out why this is buggy and why it throws the server to a different position than it is supposed to.
    public static bool dead = false;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        s_maxHealth = maxHealth;
        health = maxHealth;
        GameUI.instance.leftTopSlider.maxValue = maxHealth;
        GameUI.instance.leftTopSlider.value = GameUI.instance.leftTopSlider.maxValue;
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

    /// <summary>
    /// Called when an enemy hits the server. Particles spawn to visualize the damage. On 0 health, server spawns to its checkpoint position and its HP resets.
    /// </summary>
    /// <param name="enemyCol"></param>
    /// <returns></returns>
    private IEnumerator OnDamageTaken(Collision2D enemyCol)
    {
        ShapeModule s = particleSystem.shape;
        s.position = new Vector3((enemyCol.transform.position.x - transform.position.x) / transform.lossyScale.x, s.position.y, s.position.z);
        particleSystem.Play();

        enemyCol.gameObject.GetComponent<Collider2D>().enabled = false;
        enemyCol.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        health--;
        GameUI.instance.leftTopSlider.value--;
        if (health == 0)
        {
            dead = true;
            health = maxHealth;
            GameUI.instance.leftTopSlider.value = health;
            transform.position = checkpointPosition;
        }

        canTakeDmg = false;
        yield return new WaitForSeconds(1f);
        canTakeDmg = true;
    }
}
