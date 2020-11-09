using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNS = GameNS;

public class PlayerLvl02MatFiz : Player
{
    [SerializeField] BallLvl02MatekFizika ball = default;
    public static bool isBallOnScreen = true;
    float leftScreenBound = 0f, rightScreenBound = 0f;
    public static int brickCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        reachedEnd = false;

        gradeAllSum.count = 0;
        gradeAllSum.maxSum = 0;
        Grade[] grades = FindObjectsOfType<Grade>();

        for (int i = 0; i < grades.Length; i++)
        {
            gradeAllSum.maxSum += (int)grades[i].nem;
        }

        quizMaxAll.correctQuestions = 0;
        quizMaxAll.allQuestions = FindObjectsOfType<quizCollider>().Length;

        for (int levelIndex = 0; levelIndex < 5; levelIndex++)
        {
            if (PlayerPrefs.GetFloat("FGrade" + levelIndex, 100f) != 100f)
            {
                Debug.Log("Float grade " + levelIndex + ": " + PlayerPrefs.GetFloat("FGrade" + levelIndex));
            }
        }

        GameUI.loads = false;
        Quiz.checkpoint = null;
        brickCount = FindObjectsOfType<ObstacleLvl02>().Length;

        ball.gameObject.SetActive(false);

        leftScreenBound = Camera.main.transform.position.x - ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2) + halfPlayerSize.x;
        rightScreenBound = Camera.main.transform.position.x + ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2) - halfPlayerSize.x;

        cameraOffset = Camera.main.transform.position - transform.position;
        halfPlayerSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x / 2, GetComponent<SpriteRenderer>().bounds.size.y / 2);
        StartCoroutine(WaitForOK());
    }

    IEnumerator WaitForOK()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameNS::StaticData.gameUI.scoreCountText.GetComponent<Score>().OnGameLevelOpen();
        ball.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        ball.enabled = !quizCollider.quizActive;

        if (brickCount > 0)
        {
            if (!isBallOnScreen)
            {
                OnGameOver();
                isBallOnScreen = true;
            }

            if (!reachedEnd && !quizCollider.quizActive && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.position -= new Vector3(10f * Time.deltaTime, 0f);
                }
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    transform.position += new Vector3(10f * Time.deltaTime, 0f);
                }

                transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftScreenBound, rightScreenBound), transform.position.y);
            }
        }
        else if(!quizCollider.quizActive) // No quiz is going and there aren't any bricks on the scene
        {
            ball.enabled = false;
            reachedEnd = true;
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
