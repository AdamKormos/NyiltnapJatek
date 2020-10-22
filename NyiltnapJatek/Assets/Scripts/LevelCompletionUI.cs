using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class LevelCompletionUI : MonoBehaviour
{
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

        GameNS::StaticData.gameUI.levelCompletionPanelText.text =
            GameNS::StaticData.gameUI.scoreCountText.text + '\n' +
            quizMaxAll.correctQuestions + " / " + quizMaxAll.allQuestions + '\n' +
            gradeAllSum.sum + " / " + gradeAllSum.maxSum;
    }
}
