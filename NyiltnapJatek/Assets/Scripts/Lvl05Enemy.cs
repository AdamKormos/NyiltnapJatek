using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Enemy class for Lvl05.
/// </summary>
public class Lvl05Enemy : MonoBehaviour
{
    [SerializeField] public byte health = 2;
    [SerializeField] public int scoreReward = 100;
    protected byte maxHealth;
    protected bool hasHealthSlider = false;
    protected Slider attachedSlider = default;

    private void Start()
    {
        maxHealth = health;
    }

    /// <summary>
    /// Behaves as such because of the checkpoint simulation, as now, if enemies appear on the screen, they have full health. If they were to be killed, enemies wouldn't
    /// appear again if they were killed but the player spawned at a checkpoint.
    /// </summary>
    private void OnBecameVisible()
    {
        health = maxHealth;

        if (hasHealthSlider)
        {
            foreach (Image img in attachedSlider.transform.parent.GetComponentsInChildren<Image>(true))
            {
                img.enabled = true;
            }

            attachedSlider.value = maxHealth;
        }
    }

    private void OnBecameInvisible()
    {
        if (hasHealthSlider)
        {
            foreach (Image img in attachedSlider.transform.parent.GetComponentsInChildren<Image>(true))
            {
                img.enabled = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>())
        {
            health--;

            if (!hasHealthSlider)
            {
                GameObject healthSlider = Instantiate(GameUI.instance.lvl05HealthIndicatorSliderObject.gameObject, transform.position + new Vector3(0, 1f, 0f), Quaternion.identity, this.transform);
                healthSlider.gameObject.SetActive(true);
                attachedSlider = healthSlider.GetComponentInChildren<Slider>();
                attachedSlider.maxValue = maxHealth;
                hasHealthSlider = true;
            }
            attachedSlider.value = health;

            Destroy(collision.gameObject);
            if (health == 0) SimulateDeathAndApplyPlusScore();
        }
    }

    /// Called when the enemy has 0 health. Truly, it's not getting destroyed, only the collision and the sprite gets disabled so that itt can reappear once the player 
    /// respawned at a checkpoint.
    protected void SimulateDeathAndApplyPlusScore()
    {
        GameUI.instance.scoreCountText.text = (System.Convert.ToInt32(GameUI.instance.scoreCountText.text) + scoreReward).ToString();
        
        foreach(Collider2D col in GetComponents<Collider2D>()) col.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        foreach(Image img in attachedSlider.transform.parent.GetComponentsInChildren<Image>(true))
        {
            img.enabled = false;
        }
    }
}
