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
    [SerializeField] GameObject serverObject = default;
    float leftScreenBound = 0f, rightScreenBound = 0f;
    public static int bulletCount { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        bulletCount = 50;
        GameNS::StaticData.gameUI.bulletCountText.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.bulletCountText.text = bulletCount.ToString();

        halfPlayerSize = GetComponent<SpriteRenderer>().bounds.size.y / 2;

        leftScreenBound = Camera.main.transform.position.x - ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2) + halfPlayerSize;
        rightScreenBound = Camera.main.transform.position.x + ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2) - halfPlayerSize;

        startPos = transform.position;
        cameraStartPos = Camera.main.transform.position;

        levelCompletionPanelParent = GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.GetComponent<LevelCompletionUI>();
        if (levelCompletionPanelParent != null) levelCompletionPanelParent.CallPanel(false);

        GameNS::StaticData.gameUI.LoadLevelHint("Védd meg a szervereket az ellenfelek elpusztításával! A szóközzel tudsz lőni.");
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
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
                GameObject bullet = Instantiate(bulletObject, transform.position + new Vector3(0, halfPlayerSize + 0.1f, -5f), Quaternion.identity);
                bulletCount--;
                GameNS::StaticData.gameUI.bulletCountText.text = bulletCount.ToString();
            }
        }
    }

    protected override IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameNS::StaticData.gameUI.scoreCountText.GetComponent<Score>().OnGameLevelOpen(Menu.Scenes.Lvl5);

        while (!reachedEnd)
        {
            if (moveAllowed)
            {
                transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
                serverObject.transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
                Camera.main.transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
                yield return new WaitForSeconds(1f / movePerSec);
            }
            else yield return new WaitForSeconds(0.1f);
        }

        while (isOnScreen)
        {
            transform.position += new Vector3(0f, 0.05f * moveStrength) * Time.deltaTime;
            yield return new WaitForSeconds(1f / movePerSec);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Z");
        if (collision.tag.Equals("BulletClip"))
        {
            bulletCount += Random.Range(minimumBulletFromClip, maximumBulletFromClip);
            GameNS::StaticData.gameUI.bulletCountText.text = bulletCount.ToString();
            Destroy(collision.gameObject);
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
            else if (transform.position.y < Camera.main.transform.position.y + Camera.main.orthographicSize)
            {
                reachedEnd = false; // Setting it back to false for further levels

                if (levelCompletionPanelParent != null)
                {
                    LevelSelection.OnLevelCompleted();
                    GameNS::StaticData.gameUI.bulletCountText.gameObject.SetActive(false);
                    levelCompletionPanelParent.CallPanel(true);
                }
            }
        }
    }
}
