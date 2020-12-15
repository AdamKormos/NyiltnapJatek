using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The class used for checkpoints. It contains a position, a score and an array for other things like line index on Lvl03.
/// </summary>
public class Checkpoint
{
    public Vector2 position;
    public string score;
    public string[] other;

    public Checkpoint(Vector2 pos, string sc, params string[] rest)
    {
        position = pos;
        score = sc;
        other = rest;
    }
}

/// <summary>
/// Class for handling quiz events.
/// </summary>
public class Quiz : MonoBehaviour
{
    [SerializeField] float quizAnswerTimeLimit = 10f;
    [SerializeField] Slider countdownIndicator = default;
    [SerializeField] int countdownSliderTickPerSec = 30;
    [SerializeField] Color hoveredAnswerOptionColor = default;
    [SerializeField] private List<Button> but = new List<Button>(4);
    public static Checkpoint checkpoint = null;
    private static int rowIndex = 0, colIndex = 0, correctIndex = 0;
    private static string[] answerList = new string[4];
    WaitForSeconds sliderDecrWait;

    private void OnEnable()
    {
        countdownIndicator.value = 100f;
        StartCoroutine(LoadQuestionTexts());
    }

    /// <summary>
    /// Loads the given answer list into the text children.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadQuestionTexts()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < but.Count; i++)
        {
            but[i].GetComponentInChildren<Text>(true).text = answerList[i];
        }
    }

    private void Start()
    {
        sliderDecrWait = new WaitForSeconds(1f / countdownSliderTickPerSec);
        countdownIndicator.maxValue = 100f;
    }

    private void Update()
    {
        if (quizCollider.quizActive && !Menu.isMenuImgActive)
        {
            but[rowIndex + colIndex].GetComponent<Image>().color = new Color(1f, 1f, 1f);

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) rowIndex -= 2;
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) rowIndex += 2;
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) colIndex--;
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) colIndex++;

            rowIndex = Mathf.Clamp(rowIndex, 0, 2);
            colIndex = Mathf.Clamp(colIndex, 0, 1);

            but[rowIndex + colIndex].GetComponent<Image>().color = hoveredAnswerOptionColor;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (rowIndex + colIndex == correctIndex)
                {
                    quizMaxAll.correctQuestions++;
                    CreateCheckpoint();
                    GameUI.instance.StartCoroutine(DisplayCorrectText()); // As this object would be inactive by the time the coroutine does something after the delay
                }

                CloseQuiz();
            }
        }
    }

    /// <summary>
    /// Creates a checkpoint. Called on good answer.
    /// </summary>
    private void CreateCheckpoint()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            checkpoint = new Checkpoint(Player.currentPosition, GameUI.instance.scoreCountText.text, PlayerLvl03Muveszetek.index.ToString());
        }
        else checkpoint = new Checkpoint(Player.currentPosition, GameUI.instance.scoreCountText.text);
    }

    /// <summary>
    /// Displays a text to indicate that the answer was good if that's the case. Stays active for a second.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayCorrectText()
    {
        GameUI.instance.correctAnswerText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameUI.instance.correctAnswerText.gameObject.SetActive(false);
    }

    /// <summary>
    /// The first method to be called when a quiz event is supposed to happen. Stores the necessary things for quizes and freezes player movement.
    /// </summary>
    /// <param name="questionName"></param>
    /// <param name="answers"></param>
    /// <param name="correctAnswerIndex"></param>
    public static void InitiateQuiz(string questionName, string[] answers, byte correctAnswerIndex)
    {
        GameUI.instance.quizQuestionText.text = questionName;
        answerList = answers;
        correctIndex = correctAnswerIndex;
        Player.moveAllowed = false;
    }

    /// <summary>
    /// Called when the quiz begins. Initiates the countdown progress bar's decrementation.
    /// </summary>
    /// <returns></returns>
    public IEnumerator QuizCountdown()
    {
        float toDecrementPerTick = countdownIndicator.maxValue / (quizAnswerTimeLimit * countdownSliderTickPerSec);

        while (quizCollider.quizActive && countdownIndicator.value > 0)
        {
            countdownIndicator.value -= toDecrementPerTick;
            yield return sliderDecrWait;
        }
        
        if(quizCollider.quizActive) CloseQuiz();
    }

    /// <summary>
    /// Called when quiz is over. Resets quiz button colors and indeces and allows movement for the player.
    /// </summary>
    private void CloseQuiz()
    {
        but[rowIndex + colIndex].GetComponent<Image>().color = new Color(1f, 1f, 1f);
        rowIndex = 0;
        colIndex = 0;

        transform.parent.gameObject.SetActive(false);
        Player.moveAllowed = true;
        quizCollider.quizActive = false;
    }
}