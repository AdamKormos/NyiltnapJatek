using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameNS = GameNS;

public class Menu : MonoBehaviour
{
    [SerializeField] Color selectedButtonColor = default;
    [SerializeField] private Button[] buttons = new Button[3];
    [SerializeField] public GameObject menuImg = default;
    private int index = 0;
    public static bool isMenuImgActive { get; private set; }

    private void OnEnable()
    {
        buttons[index].GetComponent<Image>().color = new Color(1f, 1f, 1f);
        index = 0;
        buttons[index].GetComponent<Image>().color = selectedButtonColor;
    }

    void Update()
    {
        isMenuImgActive = menuImg.activeSelf;

        if (Input.GetKeyDown(KeyCode.Escape) && !menuImg.activeSelf)
        {
            menuImg.SetActive(!menuImg.activeSelf);
            GameNS::StaticData.gameUI.levelSelectionTransform.gameObject.SetActive(!menuImg.activeSelf);
            GameNS::StaticData.gameUI.creditsTransform.gameObject.SetActive(!menuImg.activeSelf);
        }
        if (menuImg.activeSelf)
        {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && index != 0)
            {
                buttons[index].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                index--;
                buttons[index].GetComponent<Image>().color = selectedButtonColor;
            }
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && index != 2)
            {
                buttons[index].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                index++;
                buttons[index].GetComponent<Image>().color = selectedButtonColor;
            }
            else if(Input.GetKeyDown(KeyCode.Return))
            {
                menuImg.SetActive(!menuImg.activeSelf);
                switch (index)
                {
                    case 0:
                        OnLevelSelectionPress();
                        break;
                    case 1:
                        Credits();
                        break;
                    case 2:
                        Exit();
                        break;
                }
            }
        }
    }
    
    private void OnLevelSelectionPress()
    {
        GameNS::StaticData.gameUI.levelSelectionTransform.gameObject.SetActive(true);
    }

    private void Credits()
    {
        GameNS::StaticData.gameUI.creditsTransform.gameObject.SetActive(true);
    }

    private void Exit()
    {
        Application.Quit();
    }
}
