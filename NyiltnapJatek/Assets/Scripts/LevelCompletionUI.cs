using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class LevelCompletionUI : MonoBehaviour
{
    public static int calculatedGrade; // Calculation @ Score.cs

    private void Start()
    {
        if (gradeAllSum.maxSum == 0)
        {
            Grade[] grades = FindObjectsOfType<Grade>();

            for (int i = 0; i < grades.Length; i++)
            {
                gradeAllSum.maxSum += (int)grades[i].nem;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf && (Player.reachedEnd || PlayerLvl02MatFiz.brickCount < 1))
        {
            PlayerLvl02MatFiz.brickCount = 1;
            GameNS::StaticData.loadingScreen.LoadLevel(0); // Main Menu
        }
    }

    public void CallPanel(bool activityState)
    {
        GameUI.ToggleChildren(this.gameObject, activityState);

        if (activityState)
        {
            GameNS::StaticData.gameUI.levelCompletionPanelText.text =
                GameNS::StaticData.gameUI.scoreCountText.text + '\n' +
                quizMaxAll.correctQuestions + " / " + quizMaxAll.allQuestions + '\n' +
                gradeAllSum.sum + " / " + gradeAllSum.maxSum + '\n' + '\n' + '\n' +
                calculatedGrade;
        }
    }
}
