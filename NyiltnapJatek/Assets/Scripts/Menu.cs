using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameNS = GameNS;

public class Menu : MonoBehaviour
{
    public enum Scenes {mainMenu, sampleScene, zoldMap, Lvl1, Lvl2, Lvl3, Lvl4, Lvl5 };
    [SerializeField] private Button[] buttons = new Button[3];
    [SerializeField] public GameObject menuImg = default;
    private int index = 0;
    public static bool isMenuImgActive { get; private set; }

    void Update()
    {
        isMenuImgActive = menuImg.activeSelf;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuImg.SetActive(!menuImg.activeSelf);
            GameNS::StaticData.gameUI.levelSelectionTransform.gameObject.SetActive(!menuImg.activeSelf);
        }
        if (menuImg.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                buttons[index].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                index--;
                if (index < 0)
                {
                    index++;
                }
                buttons[index].GetComponent<Image>().color = new Color(0.1f, 0.3f, 0.4f);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                buttons[index].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                index++;
                if (index > 2)
                {
                    index--;
                }
                buttons[index].GetComponent<Image>().color = new Color(0.1f, 0.3f, 0.4f);
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

    }

    private void Exit()
    {
        SceneManager.LoadScene((int)Scenes.mainMenu);
    }
}
