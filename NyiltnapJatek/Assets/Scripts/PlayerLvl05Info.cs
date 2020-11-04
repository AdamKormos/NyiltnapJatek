﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class PlayerLvl05Info : Player
{
    [SerializeField] byte minimumBulletFromClip = 5;
    [SerializeField] byte maximumBulletFromClip = 20;
    [SerializeField] float xMoveStrength = 2f;
    [SerializeField] KeyCode shootKey = KeyCode.Space;
    [SerializeField] GameObject bulletObject = default;
    [SerializeField] Lvl05Server serverObject = default;
    float leftScreenBound = 0f, rightScreenBound = 0f;
    public static int bulletCount { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
#else
        moveStrength = 30;
        xMoveStrength *= 2f;
#endif

        bulletCount = 50;
        GameNS::StaticData.gameUI.lvl05StuffTransform.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.bulletCountText.text = bulletCount.ToString();

        leftScreenBound = Camera.main.transform.position.x - ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2) + halfPlayerSize.x;
        rightScreenBound = Camera.main.transform.position.x + ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2) - halfPlayerSize.x;

        GameNS::StaticData.gameUI.leftTopSlider.maxValue = serverObject.maxHealth;
        GameNS::StaticData.gameUI.leftTopSlider.value = GameNS::StaticData.gameUI.leftTopSlider.maxValue;

        cameraOffset = Camera.main.transform.position - transform.position;
        halfPlayerSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x / 2, GetComponent<SpriteRenderer>().bounds.size.y / 2);
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;

        if (Lvl05Server.health <= 0)
        {
            OnGameOver();
            EnableEnemies();
            StartCoroutine(RespawnFrameDelay());
        }

        if (!reachedEnd && !quizCollider.quizActive && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf)
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
                GameNS::StaticData.gameUI.bulletCountText.text = bulletCount.ToString();
            }
        }
    }

    IEnumerator RespawnFrameDelay()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        respawnedAtCheckpoint = false;
    }

    protected override IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameNS::StaticData.gameUI.scoreCountText.GetComponent<Score>().OnGameLevelOpen();

        GameNS::StaticData.gameUI.leftTopSlider.gameObject.SetActive(true);
        
        while (!reachedEnd)
        {
            if (moveAllowed)
            {
                transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
                serverObject.transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
                Camera.main.transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            else yield return new WaitForSeconds(0.1f);
        }

        while (isOnScreen)
        {
            transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("LevelEnding")) { reachedEnd = true; }
        else if (collision.tag.Equals("BulletClip"))
        {
            bulletCount += Random.Range(minimumBulletFromClip, maximumBulletFromClip);
            GameNS::StaticData.gameUI.bulletCountText.text = bulletCount.ToString();
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

    private static void EnableEnemies()
    {
        foreach (Lvl05Enemy g in FindObjectsOfType<Lvl05Enemy>())
        {
            g.GetComponent<Collider2D>().enabled = true;
            g.GetComponent<SpriteRenderer>().enabled = true;
        }

        foreach (Lvl05SpaceshipEnemy g in FindObjectsOfType<Lvl05SpaceshipEnemy>())
        {
            g.GetComponent<Collider2D>().enabled = true;
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
                GameNS::StaticData.gameUI.lvl05StuffTransform.gameObject.SetActive(false);

                Score.CalculateResults();
                LevelSelection.OnLevelCompleted();
            //}
        }
    }
}
