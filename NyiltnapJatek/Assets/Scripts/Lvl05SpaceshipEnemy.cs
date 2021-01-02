using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for enemy spaceships for Lvl05.
/// </summary>
public class Lvl05SpaceshipEnemy : Lvl05Enemy
{
    [SerializeField] GameObject bullet = default;
    bool isOnScreen = false;
    float halfSpriteHeight = 0f;

    private void Start()
    {
        maxHealth = health;
        halfSpriteHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;

        health = maxHealth;

        if (hasHealthSlider)
        {
            foreach (Image img in attachedSlider.transform.parent.GetComponentsInChildren<Image>(true))
            {
                img.enabled = true;
            }

            attachedSlider.value = maxHealth;
        }

        StartCoroutine(Shoot());
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;

        if (hasHealthSlider)
        {
            foreach (Image img in attachedSlider.transform.parent.GetComponentsInChildren<Image>(true))
            {
                img.enabled = false;
            }
        }
    }

    /// <summary>
    /// Spawns a bullet every x seconds. Bullets have EnemyBullet (gotta change to ConstantMovement) script on them.
    /// </summary>
    /// <returns></returns>
    IEnumerator Shoot()
    {
        while(isOnScreen)
        {
            GameObject b = Instantiate(bullet.gameObject, transform.position - new Vector3(0, halfSpriteHeight, 0), Quaternion.Euler(0f, 0f, -180f), transform.parent);
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }
}
