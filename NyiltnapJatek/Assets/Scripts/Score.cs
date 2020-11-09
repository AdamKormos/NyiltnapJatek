﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameNS = GameNS;

public class Score : MonoBehaviour
{
    public static int value { get; set; }
    public static int tenth { get; private set; }
    public static bool isPaused = false;

    public void OnGameLevelOpen()
    {
        GameNS::StaticData.gameUI.scoreCountText.gameObject.SetActive(true);
        GameNS::StaticData.gameUI.keyGuide.gameObject.SetActive(true);
        value = 0;

        if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            GameNS::StaticData.gameUI.lvl05StuffTransform.gameObject.SetActive(true);
        }
        else
        {
            StopCoroutine("Count");
            tenth = 0;
            StartCoroutine("Count");
        }
    }

    public IEnumerator Count()
    {
        // The "value" variable represents seconds here.
        tenth = 0;
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

        enemy.GetComponent<Collider2D>().enabled = false;
        enemy.GetComponent<SpriteRenderer>().enabled = false;
    }

    public static void OnEnemyKilled(Lvl05SpaceshipEnemy enemy)
    {
        GameNS::StaticData.gameUI.scoreCountText.text = (System.Convert.ToInt32(GameNS::StaticData.gameUI.scoreCountText.text) + enemy.scoreReward).ToString();

        enemy.GetComponent<Collider2D>().enabled = false;
        enemy.GetComponent<SpriteRenderer>().enabled = false;
    }

    public static void CalculateResults()
    {
        float percentage = ((((float)quizMaxAll.correctQuestions / (float)quizMaxAll.allQuestions * 3f) 
            + ((float)gradeAllSum.sum / (float)gradeAllSum.maxSum) * 2f));

        percentage /= 5f;
        int grade = (int)(percentage / 0.2f) + 1;
        LevelCompletionUI.calculatedGrade = Mathf.Clamp(grade, 1, 5);

        if (RandomAccessFile.LoadData(LevelSelection.currentSceneIndex) == null || 
            (RandomAccessFile.LoadData(LevelSelection.currentSceneIndex) != null && (int)RandomAccessFile.LoadData(LevelSelection.currentSceneIndex).Item2 > LevelCompletionUI.calculatedGrade))
        {
            PlayerPrefs.SetFloat("FGrade" + LevelSelection.currentSceneIndex, Mathf.Clamp(percentage / 0.2f, 1.00f, 5.00f));
            PlayerPrefs.Save();
        }
        
        if(SceneManager.GetActiveScene().buildIndex == 2) LevelSelection.OnLevelCompleted();

        LevelSelection.FetchCompletionData(tenth, (gradeAllSum.gradeEnum)LevelCompletionUI.calculatedGrade);
    }
}
