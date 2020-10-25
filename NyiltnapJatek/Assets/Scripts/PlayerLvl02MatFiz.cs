using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNS = GameNS;

public class PlayerLvl02MatFiz : Player
{
    [SerializeField] BallLvl02MatekFizika ball = default;
    public static bool isBallOnScreen = true;

    // Start is called before the first frame update
    void Start()
    {
        ball.gameObject.SetActive(false);

        levelCompletionPanelParent = GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.GetComponent<LevelCompletionUI>();
        if (levelCompletionPanelParent != null) levelCompletionPanelParent.CallPanel(false);

        StartCoroutine(WaitForOK());
    }

    IEnumerator WaitForOK()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameNS::StaticData.gameUI.scoreCountText.GetComponent<Score>().OnGameLevelOpen(Menu.Scenes.Lvl2);
        ball.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBallOnScreen)
        {
            OnGameOver();
            isBallOnScreen = true;
        }

        if (!reachedEnd && !quizCollider.quizActive && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.position -= new Vector3(10f * Time.deltaTime, 0f);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(10f * Time.deltaTime, 0f);
            }
        }
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;
    }
}
