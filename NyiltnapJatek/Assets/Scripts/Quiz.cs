using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] private int correctIndex = 0;
    [SerializeField] private List<Button> but = new List<Button>(4);
    private static int rowIndex = 0, colIndex = 0;

    private void Start()
    {
        quizMaxAll.allQuestions = FindObjectsOfType<quizCollider>().Length;
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

            but[rowIndex + colIndex].GetComponent<Image>().color = new Color(0.1f, 0.3f, 0.4f);

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (rowIndex + colIndex == correctIndex)
                {
                    Debug.Log("Nice");
                    quizMaxAll.correctQuestions++;
                }
                else
                {
                    Debug.Log("Bad");
                }

                transform.parent.gameObject.SetActive(false);
                Player.moveAllowed = true;
                Timer.isPaused = false;
                quizCollider.quizActive = false;
            }
        }
    }
}



