using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Lvl05Server : MonoBehaviour
{
    [SerializeField] byte maxHealth = 20;
    byte health = 0;

    private void Start()
    {
        health = maxHealth;
        GetComponent<ParticleSystem>().Stop();    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Lvl05Enemy>())
        {
            OnDamageTaken(collision);
        }
    }

    private void OnDamageTaken(Collision2D enemyCol)
    {
        ShapeModule s = GetComponent<ParticleSystem>().shape;
        s.position = new Vector3((enemyCol.transform.position.x - transform.position.x) / 100f, s.position.y, s.position.z);
        GetComponent<ParticleSystem>().Play();
        Destroy(enemyCol.gameObject);

        health--;
        if(health == 0)
        {
            health = maxHealth; // Reset HP
            PlayerLvl05Info.OnServersBurning();
        }
    }
}
