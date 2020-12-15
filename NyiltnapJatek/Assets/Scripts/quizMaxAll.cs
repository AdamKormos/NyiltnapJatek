using UnityEngine;

/// <summary>
/// Class that holds information related to quizes.
/// </summary>
public class quizMaxAll : MonoBehaviour
{
    public static int allQuestions = 0;
    public static int correctQuestions = 0;

    private void OnEnable()
    {
        if (quizCollider.quizActive) StartCoroutine(GetComponentInChildren<Quiz>().QuizCountdown());
    }

    private void Start()
    {
        allQuestions = FindObjectsOfType<quizCollider>().Length;
        //Debug.Log(allQuestions);
    }
}
