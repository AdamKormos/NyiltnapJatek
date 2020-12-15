using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// The class of the loading screen. Responsible for loading levels and displaying the loading screen UI.
/// </summary>
public class LoadingScreen : MonoBehaviour
{
    [SerializeField] float minimumLoadTime = 4f, maximumLoadTime = 10f;
    [SerializeField] Text hintText = default;
    [SerializeField] string[] hints = default;
    public static bool finishedLoading { get; set; }
    public static bool startedLoading { get; private set; }
    public static LoadingScreen instance = default;

    private void Start()
    {
        startedLoading = false;
        GameUI.ToggleChildren(this.gameObject, false);
        instance = this;
    }

    /// <summary>
    /// Loads a given level and enables this object's children (= displays loading screen UI).
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadLevel(int sceneIndex)
    {
        startedLoading = true;
        finishedLoading = false;
        hintText.text = "";
        GameUI.ToggleChildren(this.gameObject, true);

        GameUI.instance.mainMenuTransform.gameObject.SetActive(false);
        GameUI.instance.levelSelectionTransform.gameObject.SetActive(false);
        GameUI.instance.levelCompletionPanelText.transform.parent.gameObject.SetActive(false);

        SceneManager.LoadScene(sceneIndex);

        ResetNeededValuesAndRecountGrades();

        quizMaxAll.correctQuestions = 0;
        quizMaxAll.allQuestions = FindObjectsOfType<quizCollider>().Length;

        GameUI.instance.scoreCountText.gameObject.SetActive(false);
        GameUI.instance.leftTopSlider.gameObject.SetActive(false);
        GameUI.instance.quizTransform.gameObject.SetActive(false);
        GameUI.instance.bulletCountText.gameObject.SetActive(false);
        GameUI.instance.correctAnswerText.gameObject.SetActive(false);
        StartCoroutine(Load(sceneIndex == 0)); // Is Scene Index the Main Menu Index?
    }

    /// <summary>
    /// Called when a new level is opened. Resets values that should be resetted when entering a new level.
    /// </summary>
    private static void ResetNeededValuesAndRecountGrades()
    {
        gradeAllSum.count = 0;
        gradeAllSum.maxSum = 0;
        Grade[] grades = FindObjectsOfType<Grade>();

        for (int i = 0; i < grades.Length; i++)
        {
            gradeAllSum.maxSum += (int)grades[i].nem;
        }

        Quiz.checkpoint = null;
        quizMaxAll.correctQuestions = 0;
        quizMaxAll.allQuestions = FindObjectsOfType<quizCollider>().Length;
    }

    int loadTickAmount = 250;

    /// <summary>
    /// The progress bar's loading simulation.
    /// </summary>
    /// <param name="isLoadingMainMenu"></param>
    /// <returns></returns>
    IEnumerator Load(bool isLoadingMainMenu)
    {
        GameUI.instance.keyGuide.gameObject.SetActive(false);
        GameUI.instance.scoreCountText.gameObject.SetActive(false);

        hintText.text = hints[Random.Range(0, hints.Length)];
        float val = Random.Range(minimumLoadTime, maximumLoadTime);
        Debug.Log(val);

        GameUI.instance.loadingScreenSlider.value = 0;
        GameUI.instance.loadingScreenSlider.maxValue = loadTickAmount;

        for (int i = 0; i < loadTickAmount; i++)
        {
            GameUI.instance.loadingScreenSlider.value += 1;
            yield return new WaitForSeconds(val / loadTickAmount);
        }

        GameUI.loads = false;
        finishedLoading = true;
        startedLoading = false;
        GameUI.ToggleChildren(this.gameObject, false);
        GameUI.instance.OnViewChanged(isLoadingMainMenu, false);
    }
}
