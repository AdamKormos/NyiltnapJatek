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
            GameNS::StaticData.gameUI.quizTransform.gameObject.SetActive(true);
            quizActive = true;
            Quiz.InitiateQuiz(questionName, answers, correctAnswerIndex);
            Player.moveAllowed = false;
            Destroy(this.gameObject);
        }
    }
}
