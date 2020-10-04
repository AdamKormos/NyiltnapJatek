using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] private int correctIndex;
    [SerializeField] private List<Button> but = new List<Button>(4);
    private static int index = 0;
    private void Start()
    {
        MaxandCorrectQuestions.allQuestions += but.Count;    
    }

    private void Update()
    {
        if (quizCollider.quizActive)
        {
            
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                but[index].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f);
                index--;
                if (index < 0)
                {
                    index++;
                }
                but[index].GetComponent<Image>().color = new Color(0.1f, 0.3f, 0.4f);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                but[index].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f);
                index++;
                if (index > 2)
                {
                    index--;
                }
                but[index].GetComponent<Image>().color = new Color(0.1f, 0.3f, 0.4f);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (index == correctIndex)
                {
                    MaxandCorrectQuestions.correctQuestions++;
                }
                else
                {

                }
                transform.parent.parent.gameObject.GetComponent<Image>().enabled = false;
                Destroy(transform.parent.gameObject);
                Player.moveAllowed = true;

            }
        }
    }
}



