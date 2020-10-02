using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Menu : MonoBehaviour
{
    public enum Scenes {mainMenu, sampleScene, zoldMap };
    [SerializeField] private Button[] buttons = new Button[3];
    [SerializeField] private GameObject menuImg;
    private int index = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuImg.SetActive(!menuImg.activeSelf);

        }
        if (menuImg.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                buttons[index].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f);
                index++;
                if (index > 2)
                {
                    index--;
                }
                buttons[index].GetComponent<Image>().color = new Color(0.1f, 0.3f, 0.4f);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                buttons[index].GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f);
                index--;
                if (index < 0)
                {
                    index++;
                }
                buttons[index].GetComponent<Image>().color = new Color(0.1f, 0.3f, 0.4f);
            }

        }
    }



    private void Starts()
    {
        menuImg.SetActive(!menuImg.activeSelf);
    }

    private void Credits()
    {

    }

    private void Exit()
    {
        SceneManager.LoadScene((int)Scenes.mainMenu);
    }

    


}
