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
    protected Vector3 cameraStartPos, startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        cameraStartPos = Camera.main.transform.position;

        levelCompletionPanelParent = GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.GetComponent<LevelCompletionUI>();
        if (levelCompletionPanelParent != null) levelCompletionPanelParent.CallPanel(false);

        GameNS::StaticData.gameUI.LoadLevelHint("Flappy Bird - Repülj végig a pályán és ugorj a szóköz segítségével!");
        StartCoroutine(Move());
    }

    RaycastHit2D hit;
    bool isFallAllowed = true;

    // Update is called once per frame
    void Update()
    {
        if (!reachedEnd && !quizCollider.quizActive && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf)
        {
            if (isFallAllowed && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                StartCoroutine(OnJump());
            }
            else if(isFallAllowed)
            {
                transform.position += new Vector3(0, -fallStrength);
            }
        }
    }

    IEnumerator OnJump()
    {
        isFallAllowed = false;

        for(int i = 0; i < jumpTickRate; i++)
        {
            transform.position += new Vector3(0, jumpStrength / jumpTickRate);
            yield return new WaitForSeconds(jumpCooldown / jumpTickRate);
        }

        isFallAllowed = true;
    }

    protected override IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }

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
