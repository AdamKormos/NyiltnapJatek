using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class quizCollider : MonoBehaviour
{
    public static bool quizActive = false;
    private void Start()
    {
        GameNS::StaticData.gameUI.quizTransform.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            Score.isPaused = true;
            GameNS::StaticData.gameUI.quizTransform.gameObject.SetActive(true);
            quizActive = true;
            Player.moveAllowed = false;
            //Destroy(this.gameObject);
        }
    }
}
