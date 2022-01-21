using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player child for Lvl05. Spaceship with shoot ability.
/// </summary>
public class PlayerLvl05Info : Player
{
    [SerializeField] byte minimumBulletFromClip = 5;
    [SerializeField] byte maximumBulletFromClip = 20;
    [SerializeField] float xMoveStrength = 2f;
    [SerializeField] KeyCode shootKey = KeyCode.Space;
    [SerializeField] GameObject bulletObject = default;
    [SerializeField] Lvl05Server serverObject = default;
    float leftScreenBound = 0f, rightScreenBound = 0f, initialServerXPos = 0f;
    Vector3 serverOffset = default;
    public static int bulletCount { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
#else
        moveStrength = 30;
        xMoveStrength *= 2f;
#endif
        reachedEnd = false;

        initialServerXPos = serverObject.transform.position.x;
        serverOffset = transform.position - serverObject.transform.position;

        bulletCount = 50;
        GameUI.instance.lvl05StuffTransform.gameObject.SetActive(false);
        GameUI.instance.bulletCountText.text = bulletCount.ToString();

        leftScreenBound = Camera.main.transform.position.x - ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2) + halfPlayerSize.x;
        rightScreenBound = Camera.main.transform.position.x + ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2) - halfPlayerSize.x;

        cameraOffset = Camera.main.transform.position - transform.position;
        halfPlayerSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x / 2, GetComponent<SpriteRenderer>().bounds.size.y / 2);
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;
        serverObject.transform.position = new Vector3(initialServerXPos, currentPosition.y - serverOffset.y);

        if (Lvl05Server.dead)
        {
            Lvl05Server.dead = false;
            OnGameOver();
            EnableEnemies();
            respawnedAtCheckpoint = false;
        }

        if (!reachedEnd && !quizCollider.quizActive && !GameUI.instance.levelHintBar.gameObject.activeSelf)
        {
            #region Movement
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position -= new Vector3(0.05f * xMoveStrength, 0f);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(0.05f * xMoveStrength, 0f);
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftScreenBound, rightScreenBound), transform.position.y);
            #endregion

            if(Input.GetKeyDown(shootKey) && bulletCount > 0)
            {
                GameObject bullet = Instantiate(bulletObject, transform.position + new Vector3(0, halfPlayerSize.y + 0.1f, -5f), Quaternion.identity);
                bulletCount--;
                GameUI.instance.bulletCountText.text = bulletCount.ToString();
            }
        }
    }

    protected override IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameUI.instance.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameUI.instance.scoreCountText.GetComponent<Score>().OnGameLevelOpen();

        GameUI.instance.bulletCountText.gameObject.SetActive(true);
        GameUI.instance.leftTopSlider.gameObject.SetActive(true);
        
        while (!reachedEnd)
        {
            if (moveAllowed)
            {
                transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
                serverObject.transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
                Camera.main.transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
                yield return new WaitForSeconds(1 / 60);
            }
            else yield return new WaitForSeconds(0.1f);
        }

        while (isOnScreen)
        {
            transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
            yield return new WaitForSeconds(1 / 60);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelEnding")) { reachedEnd = true; }
        else if (collision.CompareTag("BulletClip"))
        {
            bulletCount += Random.Range(minimumBulletFromClip, maximumBulletFromClip);
            GameUI.instance.bulletCountText.text = bulletCount.ToString();
            Destroy(collision.gameObject);
        }
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;

        if (respawnedAtCheckpoint)
        {
            respawnedAtCheckpoint = false;
        }
    }

    /// <summary>
    /// Called at respawning.
    /// </summary>
    private static void EnableEnemies()
    {
        foreach (Lvl05Enemy g in FindObjectsOfType<Lvl05Enemy>())
        {
            foreach(Collider2D col in g.GetComponents<Collider2D>()) col.enabled = true;
            g.GetComponent<SpriteRenderer>().enabled = true;
        }

        foreach (Lvl05SpaceshipEnemy g in FindObjectsOfType<Lvl05SpaceshipEnemy>())
        {
            foreach (Collider2D col in g.GetComponents<Collider2D>()) col.enabled = true;
            g.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;

        if (Camera.main != null && reachedEnd)
        {
            //if (transform.position.y > Camera.main.transform.position.y + Camera.main.orthographicSize && reachedEnd)
            //{
                reachedEnd = false; // Setting it back to false for further levels
                GameUI.instance.lvl05StuffTransform.gameObject.SetActive(false);

                Score.CalculateResults();
                LevelSelection.OnLevelCompleted();
            //}
        }
    }
}
