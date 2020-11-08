using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class LevelCompletionUI : MonoBehaviour
{
    public static int calculatedGrade; // Calculation @ Score.cs

    bool exitAllowed = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && exitAllowed)
        {
            exitAllowed = false;
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
            exitAllowed = true;
        }
    }
}
