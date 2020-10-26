using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameNS = GameNS;

public class Score : MonoBehaviour
{
    public int value { get; private set; }
    public static int tenth { get; private set; }
    public static bool isPaused = false;

    public void OnGameLevelOpen()
    {
        value = 0;

        Menu.Scenes currentScene = (Menu.Scenes)(SceneManager.GetActiveScene().buildIndex);

        switch (currentScene)
        {
            case Menu.Scenes.Lvl1:
                StopCoroutine("Count");
                tenth = 0;
                GameNS::StaticData.gameUI.scoreCountText.text = "00:00.0";
                StartCoroutine("Count");
                break;
            case Menu.Scenes.Lvl2:
                StopCoroutine("Count");
                tenth = 0;
                GameNS::StaticData.gameUI.scoreCountText.text = "00:00.0";
                StartCoroutine("Count");
                break;
            case Menu.Scenes.Lvl3:
                GameNS::StaticData.gameUI.scoreCountText.text = "0";
                break;
            case Menu.Scenes.Lvl4:
                StopCoroutine("Count");
                tenth = 0;
                GameNS::StaticData.gameUI.scoreCountText.text = "00:00.0";
                StartCoroutine("Count");
                break;
            case Menu.Scenes.Lvl5:
                GameNS::StaticData.gameUI.scoreCountText.text = "0";
                GameNS::StaticData.gameUI.lvl05StuffTransform.gameObject.SetActive(true);
                break;
        }
    }

    public IEnumerator Count()
    {
        // The "value" variable represents seconds here.
        while(!Player.reachedEnd)
        {
            if (!isPaused)
            {
                yield return new WaitForSeconds(0.1f);
                tenth++;
                value = tenth / 10;
                GameNS::StaticData.gameUI.scoreCountText.text = ((value % 60 < 10 ? "0" : "") + (int)(value / 60) + ":" + (value % 60 < 10 ? "0" : "") + (value % 60) + "." + (tenth % 10)).ToString();
            }
            else yield return new WaitForSeconds(0.1f);
        }

        CalculateResults();
    }

    public static void OnEnemyKilled(Lvl05Enemy enemy)
    {
        GameNS::StaticData.gameUI.scoreCountText.text = (System.Convert.ToInt32(GameNS::StaticData.gameUI.scoreCountText.text) + enemy.scoreReward).ToString();
        Destroy(enemy.gameObject);
    }

    public static void CalculateResults()
    {
        float percentage = ((quizMaxAll.correctQuestions / quizMaxAll.allQuestions * 3) 
            + (gradeAllSum.sum / gradeAllSum.maxSum));

        percentage /= 4;

        int grade = (int)(percentage / 0.2f) + 1;

        LevelCompletionUI.calculatedGrade = Mathf.Clamp(grade, 1, 5);

        LevelSelection.FetchCompletionData(tenth, (gradeAllSum.gradeEnum)LevelCompletionUI.calculatedGrade);
    }
}
