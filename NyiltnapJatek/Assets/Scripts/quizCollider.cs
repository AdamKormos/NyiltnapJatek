using UnityEngine;
using UnityEngine.UI;

public class quizCollider : MonoBehaviour
{
    public static bool quizActive = false;
    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.GetComponentInParent<Image>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        quizActive = true;
        Player.moveAllowed = false;
    }
}
