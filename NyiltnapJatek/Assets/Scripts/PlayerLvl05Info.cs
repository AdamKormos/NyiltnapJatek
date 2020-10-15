using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNS = GameNS;

public class PlayerLvl05Info : Player
{
    float leftScreenBound = 0f, rightScreenBound = 0f;

    // Start is called before the first frame update
    void Start()
    {
        try { halfPlayerSize = GetComponent<BoxCollider2D>().size.y / 2; }
        catch { halfPlayerSize = GetComponent<CircleCollider2D>().radius; }

        leftScreenBound = Camera.main.transform.position.x - ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2) + halfPlayerSize;
        rightScreenBound = Camera.main.transform.position.x + ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2) - halfPlayerSize;

        startPos = transform.position;
        cameraStartPos = Camera.main.transform.position;

        levelCompletionPanelParent = GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.GetComponent<LevelCompletionUI>();
        if (levelCompletionPanelParent != null) levelCompletionPanelParent.CallPanel(false);

        GameNS::StaticData.gameUI.LoadLevelHint("Védd meg a szervereket az ellenfelek elpusztításával!");
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        if (!reachedEnd && !quizCollider.quizActive && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position -= new Vector3(0.05f * moveStrength, 0f);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(0.05f * moveStrength, 0f);
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftScreenBound, rightScreenBound), transform.position.y);
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
                transform.position += new Vector3(0f, 0.05f * moveStrength);
                Camera.main.transform.position += new Vector3(0f, 0.05f * moveStrength);
                yield return new WaitForSeconds(1f / movePerSec);
            }
            else yield return new WaitForSeconds(0.1f);
        }

        while (isOnScreen)
        {
            transform.position += new Vector3(0f, 0.05f * moveStrength);
            yield return new WaitForSeconds(1f / movePerSec);
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
                    levelCompletionPanelParent.CallPanel(true);
                }
            }
        }
    }
}
