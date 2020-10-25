using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class quizCollider : MonoBehaviour
{
    [SerializeField] string questionName = "";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] byte correctAnswerIndex = 0;

    public static bool quizActive = false;
    private void Start()
    {
        GameNS::StaticData.gameUI.quizTransform.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            quizActive = true;
            GameNS::StaticData.gameUI.quizTransform.gameObject.SetActive(true);
            Quiz.InitiateQuiz(questionName, answers, correctAnswerIndex);
            Destroy(this.gameObject);
        }
    }
}
