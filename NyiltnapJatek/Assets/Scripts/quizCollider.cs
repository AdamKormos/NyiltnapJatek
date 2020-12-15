using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The class that is used on colliders that trigger quizes.
/// </summary>
public class quizCollider : MonoBehaviour
{
    [SerializeField] string questionName = "";
    [SerializeField] string[] answers = new string[4];
    [SerializeField] byte correctAnswerIndex = 0;

    public static bool quizActive = false;
    private void Start()
    {
        GameUI.instance.quizTransform.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            if (collision.GetComponent<PlayerLvl01Human>())
            {
                if (GameUI.instance.leftTopSlider.value > 0)
                {
                    OnPlayerTouch();
                    Destroy(this.gameObject);
                }
            }
            else
            {
                OnPlayerTouch();
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Called when this collides with the player. Initiates a quiz and activates the quiz transform's gameobject.
    /// </summary>
    public void OnPlayerTouch()
    {
        quizActive = true;
        GameUI.instance.quizTransform.gameObject.SetActive(true);
        Quiz.InitiateQuiz(questionName, answers, correctAnswerIndex);
    }
}
