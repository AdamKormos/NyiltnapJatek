using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    private Collision2D collArray;
    private static Image img;
    [SerializeField] private int correctIndex;
    [SerializeField] private List<Button> but = new List<Button>(4);
    private static int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        collArray = transform.GetComponentInChildren<Collision2D>();
        img = transform.GetComponent<Image>();
        img.enabled = false;
        foreach (Button i in but)
        {
            i.gameObject.SetActive(false);
        }

    }
    private void OnCollision2D(Collision2D collision)
    {
        img.enabled = true;
        foreach (Button i in but)
        {
            i.gameObject.SetActive(true);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
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
            if  (index == correctIndex)
            {

            }
            else
            {

            }
        }
    }

    [System.Obsolete]
    private void OnCollisionExit2D(Collision2D collision)
    {
        Destroy(this);
    }
}


public class quizArray : MonoBehaviour
{
    [SerializeField] private Quiz[] quizArr = new Quiz[3];
}



