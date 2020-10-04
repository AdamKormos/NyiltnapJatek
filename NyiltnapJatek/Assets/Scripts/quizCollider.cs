using UnityEngine;
using UnityEngine.UI;

public class quizCollider : MonoBehaviour
{
    [SerializeField] Transform quizUI = default;

    public static bool quizActive = false;
    private void Start()
    {
        quizUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            Timer.isPaused = true;
            quizUI.gameObject.SetActive(true);
            quizActive = true;
            Player.moveAllowed = false;
        }
    }
}
