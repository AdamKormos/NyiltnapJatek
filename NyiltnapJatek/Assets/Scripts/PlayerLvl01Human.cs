using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player child for Lvl01. Fly ability with wax pickups.
/// </summary>
public class PlayerLvl01Human : Player
{
    [SerializeField] protected float fallStrength = 0.05f;
    [SerializeField] protected float jumpStrength = 3;
    [SerializeField] protected float wingHealth = 100f;
    [SerializeField] protected float wingHealthIncreaseOnWaxPickup = 25f;
    [SerializeField] protected float wingHealthDecreasePerFrame = 0.05f;

    public static float s_wingHealth { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        /*
#if UNITY_EDITOR
#else
        moveStrength = 60;
        fallStrength = 0.04f;
        jumpStrength = 0.06f;
        wingHealthDecreasePerFrame = 0.2f;
        wingHealthIncreaseOnWaxPickup = 40;
#endif
        */
        reachedEnd = false;

        s_wingHealth = wingHealth;
        GameUI.instance.leftTopSlider.maxValue = wingHealth;
        GameUI.instance.leftTopSlider.value = GameUI.instance.leftTopSlider.maxValue;

        cameraOffset = Camera.main.transform.position - transform.position;
        halfPlayerSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x / 2, GetComponent<SpriteRenderer>().bounds.size.y / 2);
        StartCoroutine(Move());
    }

    RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        if (!reachedEnd && !quizCollider.quizActive && !GameUI.instance.levelHintBar.gameObject.activeSelf)
        {
            if (Input.GetKey(KeyCode.Space) && GameUI.instance.leftTopSlider.value > 0)
            {
                transform.position += new Vector3(0, jumpStrength);
            }
            else
            {
                transform.position += new Vector3(0, -fallStrength);
            }

            hit = Physics2D.Raycast(transform.position + new Vector3(0, halfPlayerSize.y + 1f), Vector2.up);

            if ((hit.transform == null || !hit.transform.CompareTag("Cloud")) && !reachedEnd)
            {
                GameUI.instance.leftTopSlider.value -= wingHealthDecreasePerFrame;
            }
        }

        currentPosition = transform.position;
    }

    Vector3 positionToAddOnFrame;

    protected override IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameUI.instance.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameUI.instance.scoreCountText.GetComponent<Score>().OnGameLevelOpen();

        GameUI.instance.leftTopSlider.gameObject.SetActive(true);

        while (!reachedEnd)
        {
            if (moveAllowed)
            {
                positionToAddOnFrame = new Vector3(0.05f * moveStrength, 0f) * 0.0065f /*Time.deltaTime*/
                    * (0.7f + ((GameUI.instance.leftTopSlider.value / GameUI.instance.leftTopSlider.maxValue / 10) * 3));
                transform.position += positionToAddOnFrame;
                Camera.main.transform.position += positionToAddOnFrame;
                yield return new WaitForSeconds(1 / 60);
            }
            else yield return new WaitForSeconds(0.1f);
        }

        while (isOnScreen)
        {
            transform.position += new Vector3(0.05f * moveStrength, 0f) * Time.deltaTime;
            yield return new WaitForSeconds(1 / 60);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelEnding")) { reachedEnd = true; }
        else if (collision.CompareTag("Wax"))
        {
            GameUI.instance.leftTopSlider.value = Mathf.Clamp(GameUI.instance.leftTopSlider.value + wingHealthIncreaseOnWaxPickup, 0, GameUI.instance.leftTopSlider.maxValue);
            collision.GetComponent<BoxCollider2D>().enabled = false;
            collision.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (collision.CompareTag("PassiveEnemy") && moveAllowed) OnGameOver();
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
        
        if (respawnedAtCheckpoint)
        {
            foreach(GameObject g in GameObject.FindGameObjectsWithTag("Wax"))
            {
                g.GetComponent<BoxCollider2D>().enabled = true;
                g.GetComponent<SpriteRenderer>().enabled = true;
            }
            respawnedAtCheckpoint = false;
        }
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;

        if (Camera.main != null)
        {
            if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize)
            {
                OnGameOver();
            }
            else if (transform.position.y < Camera.main.transform.position.y + Camera.main.orthographicSize && reachedEnd)
            {
                reachedEnd = false;

                //if (GameUI.instance.levelCompletionPanelParent != null)
                //{
                    LevelSelection.OnLevelCompleted();
                //}
            }
        }
    }
}
