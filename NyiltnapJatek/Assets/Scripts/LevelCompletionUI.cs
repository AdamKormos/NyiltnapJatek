using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class LevelCompletionUI : MonoBehaviour
{
    private void Start()
    {
        Grade[] grades = FindObjectsOfType<Grade>();

        for (int i = 0; i < grades.Length; i++)
        {
            gradeAllSum.maxSum += (int)grades[i].nem;
        }
    }

    private void Update()
    {
        if(!Player.isOnScreen && Input.GetKeyDown(KeyCode.Return) && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf)
        {
            GameNS::StaticData.loadingScreen.LoadLevel(Menu.Scenes.mainMenu);
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
                gradeAllSum.sum + " / " + gradeAllSum.maxSum + '\n' + '\n' + '\n';

            float percentage = ((quizMaxAll.correctQuestions / quizMaxAll.allQuestions * 3)
                + (gradeAllSum.sum / gradeAllSum.maxSum));

            percentage /= 4;

            if (percentage < 0.2f) percentage = 1;
            else if (percentage >= 0.8f) percentage = 5;
            else if (percentage >= 0.6f) percentage = 4;
            else if (percentage >= 0.4f) percentage = 3;
            else if (percentage >= 0.2f) percentage = 2;

            GameNS::StaticData.gameUI.levelCompletionPanelText.text += percentage;

            LevelSelection.FetchCompletionData(GameNS::StaticData.gameUI.scoreCountText.text, (gradeAllSum.gradeEnum)percentage);
        }
    }
}
