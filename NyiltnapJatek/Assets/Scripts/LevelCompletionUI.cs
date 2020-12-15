using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The script of the level completion UI that appears when the player finishes a level.
/// </summary>
public class LevelCompletionUI : MonoBehaviour
{
    public static int calculatedGrade; // Calculation @ Score.cs
    bool exitAllowed = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && exitAllowed) // Pressed "OK" and allowed to leave level.
        {
            exitAllowed = false;
            LoadingScreen.instance.LoadLevel(0); // Main Menu
        }
    }

    /// <summary>
    /// Toggles the level completion panel's children. If the desired activity state is true, the method gets information about the player's results and displays it.
    /// </summary>
    /// <param name="activityState"></param>
    public void CallPanel(bool activityState)
    {
        GameUI.ToggleChildren(this.gameObject, activityState);

        if (activityState)
        {
            GameUI.instance.levelCompletionPanelText.text =
                GameUI.instance.scoreCountText.text + '\n' +
                quizMaxAll.correctQuestions + " / " + quizMaxAll.allQuestions + '\n' +
                gradeAllSum.sum + " / " + gradeAllSum.maxSum + '\n' + '\n' + '\n' +
                calculatedGrade;
            exitAllowed = true;
        }
    }
}
