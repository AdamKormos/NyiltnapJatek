using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The class used for counting the player's score.
/// </summary>
public class Score : MonoBehaviour
{
    public static int value { get; set; }
    public static int tenth { get; private set; }
    public static bool isPaused = false;

    /// <summary>
    /// Called when a level is opened. Prepares the counters.
    /// </summary>
    public void OnGameLevelOpen()
    {
        GameUI.instance.scoreCountText.gameObject.SetActive(true);
        GameUI.instance.keyGuide.gameObject.SetActive(true); // ?
        value = 0;

        if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            GameUI.instance.lvl05StuffTransform.gameObject.SetActive(true);
        }
        else
        {
            StopCoroutine("Count");
            tenth = 0;
            StartCoroutine("Count");
        }
    }

    /// <summary>
    /// Counts time.
    /// </summary>
    /// <returns></returns>
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
                GameUI.instance.scoreCountText.text = ((value % 60 < 10 ? "0" : "") + (int)(value / 60) + ":" + (value % 60 < 10 ? "0" : "") + (value % 60) + "." + (tenth % 10)).ToString();
            }
            else yield return new WaitForSeconds(0.1f);
        }

        CalculateResults();
    }

    /// <summary>
    /// Called when an enemy has 0 health. Truly, it's not getting destroyed, only the collision and the sprite gets disabled.
    /// </summary>
    /// <param name="enemy"></param>
    public static void OnEnemyKilled(Lvl05Enemy enemy)
    {
        GameUI.instance.scoreCountText.text = (System.Convert.ToInt32(GameUI.instance.scoreCountText.text) + enemy.scoreReward).ToString();

        enemy.GetComponent<Collider2D>().enabled = false;
        enemy.GetComponent<SpriteRenderer>().enabled = false;
    }

    /// <summary>
    /// Called when an enemy has 0 health. Truly, it's not getting destroyed, only the collision and the sprite gets disabled.
    /// </summary>
    /// <param name="enemy"></param>
    public static void OnEnemyKilled(Lvl05SpaceshipEnemy enemy)
    {
        GameUI.instance.scoreCountText.text = (System.Convert.ToInt32(GameUI.instance.scoreCountText.text) + enemy.scoreReward).ToString();

        enemy.GetComponent<Collider2D>().enabled = false;
        enemy.GetComponent<SpriteRenderer>().enabled = false;
    }

    /// <summary>
    /// Calculates how the player performed on the given level and assigns a grade to it.
    /// </summary>
    public static void CalculateResults()
    {
        float percentage = ((((float)quizMaxAll.correctQuestions / (float)quizMaxAll.allQuestions * 3f) 
            + ((float)gradeAllSum.sum / (float)gradeAllSum.maxSum) * 2f));

        percentage /= 5f;
        int grade = (int)(percentage / 0.2f) + 1;
        LevelCompletionUI.calculatedGrade = Mathf.Clamp(grade, 1, 5);

        if (Mathf.Clamp(percentage / 0.2f, 1.00f, 5.00f) > PlayerPrefs.GetFloat("FGrade" + LevelSelection.recentOpenedLevel, -100f))
        {
            PlayerPrefs.SetFloat("FGrade" + LevelSelection.recentOpenedLevel, Mathf.Clamp(percentage / 0.2f, 1.00f, 5.00f));
            PlayerPrefs.Save();
        }
        
        if(SceneManager.GetActiveScene().buildIndex == 2) LevelSelection.OnLevelCompleted();

        LevelSelection.FetchCompletionData(tenth, (gradeEnum)LevelCompletionUI.calculatedGrade);
    }
}
