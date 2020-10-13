using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class PlayerLvl02MatekFizika : Player
{
    [SerializeField] protected float fallStrength = 0.05f;
    [SerializeField] protected float jumpCooldown = 0.1f;
    [SerializeField] protected int jumpTickRate = 15;
    [SerializeField] protected float objectFallStrength = 0.1f;
    protected Vector3 cameraStartPos, startPos;
    RaycastHit2D hit;
    bool isJumpAllowed = true;
    float halfPlayerSize = 0f;

    // Start is called before the first frame update
    void Start()
    {
        halfPlayerSize = GetComponent<BoxCollider2D>().size.y / 2;

        startPos = transform.position;
        cameraStartPos = Camera.main.transform.position;

        levelCompletionPanelParent = GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.GetComponent<LevelCompletionUI>();
        if (levelCompletionPanelParent != null) levelCompletionPanelParent.CallPanel(false);

        GameNS::StaticData.gameUI.LoadLevelHint("Flappy Bird - Repülj végig a pályán és ugorj a szóköz segítségével!");
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        if (!reachedEnd && !quizCollider.quizActive && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf)
        {
            if (isJumpAllowed && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                StartCoroutine(OnJump());
            }
            transform.position += new Vector3(0, -fallStrength);
        }
    }

    bool canMakeObstacleFall = true;

    IEnumerator MakeObstaclesFall()
    {
        while(!reachedEnd)
        {
            if (canMakeObstacleFall)
            {
                hit = Physics2D.Raycast(transform.position + new Vector3(0, halfPlayerSize + 1f), Vector2.up);

                if (hit.transform != null)
                {
                    yield return new WaitForSeconds(Random.Range(5f, 25f));
                    StartCoroutine(OnWaitingForObstacleFall(hit.transform));
                }
                else yield return new WaitForSeconds(0.1f);
            }
            else yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator OnWaitingForObstacleFall(Transform obstacleT)
    {
        canMakeObstacleFall = false;
        
        while(true)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(0, halfPlayerSize + 1f), Vector2.up);

            if (hit.transform == null || !hit.transform.Equals(obstacleT))
            {
                break;
            }
            else yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < 200 * (objectFallStrength / 0.01f); i++)
        {
            obstacleT.transform.position -= new Vector3(0, objectFallStrength);
            yield return new WaitForEndOfFrame();
        }

        canMakeObstacleFall = true;
    }

    IEnumerator OnJump()
    {
        isJumpAllowed = false;

        fallStrength *= -1;
        yield return new WaitForSeconds(jumpCooldown);
        fallStrength *= -1;

        isJumpAllowed = true;
    }

    protected override IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }

        StartCoroutine(MakeObstaclesFall());
        GameNS::StaticData.gameUI.timerText.GetComponent<Timer>().OnGameLevelOpen();

        while (!reachedEnd)
        {
            if (moveAllowed)
            {
                transform.position += new Vector3(0.05f * moveStrength, 0f);
                Camera.main.transform.position += new Vector3(0.05f * moveStrength, 0f);
                yield return new WaitForSeconds(1f / movePerSec);
            }
            else yield return new WaitForSeconds(0.1f);
        }

        while (isOnScreen)
        {
            transform.position += new Vector3(0.05f * moveStrength, 0f);
            yield return new WaitForSeconds(1f / movePerSec);
        }
    }

    private void OnGameOver()
    {
        Timer.isPaused = false;
        GameNS::StaticData.gameUI.quizTransform.gameObject.SetActive(false);
        quizCollider.quizActive = false;
        Player.moveAllowed = true;

        transform.position = startPos;
        Camera.main.transform.position = cameraStartPos;
        startPos = transform.position;
        cameraStartPos = Camera.main.transform.position;

        GameNS::StaticData.gameUI.timerText.GetComponent<Timer>().OnGameLevelOpen();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnGameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("LevelEnding")) { reachedEnd = true; }
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
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
                if (LevelSelection.maxIndex < LevelSelection.currentIndex) LevelSelection.maxIndex = LevelSelection.currentIndex;

                if (levelCompletionPanelParent != null)
                {
                    levelCompletionPanelParent.CallPanel(true);
                }
            }
        }
    }
}
