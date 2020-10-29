using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNS = GameNS;
using static UnityEngine.ParticleSystem;

public class Lvl05Server : MonoBehaviour
{
    [SerializeField] public byte maxHealth = 20;
    public static byte health { get; private set;}
    ParticleSystem particleSystem = default;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        health = maxHealth;
        particleSystem.Stop();    
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
        ShapeModule s = particleSystem.shape;
        s.position = new Vector3((enemyCol.transform.position.x - transform.position.x) / transform.lossyScale.x, s.position.y, s.position.z);
        particleSystem.Play();
        Destroy(enemyCol.gameObject);

        health--;
        GameNS::StaticData.gameUI.leftTopSlider.value--;
    }
}
