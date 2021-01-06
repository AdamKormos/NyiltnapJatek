using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu class.
/// </summary>
public class Menu : MonoBehaviour
{
    [SerializeField] Color selectedButtonColor = default;
    [SerializeField] private Button[] buttons = new Button[3];
    [SerializeField] public GameObject menuImg = default;
    private int index = 0;
    public static bool isMenuImgActive { get; private set; }

    /// <summary>
    /// Resets current button index and button colors.
    /// </summary>
    private void OnEnable()
    {
        buttons[index].GetComponent<Image>().color = new Color(1f, 1f, 1f);
        index = 0;
        buttons[index].GetComponent<Image>().color = selectedButtonColor;
    }

    bool isInLanguageSelection = false;

    void Update()
    {
        isMenuImgActive = menuImg.activeSelf;

        if (Input.GetKeyDown(KeyCode.Escape) && !menuImg.activeSelf)
        {
            menuImg.SetActive(!menuImg.activeSelf);
            GameUI.instance.levelSelectionTransform.gameObject.SetActive(!menuImg.activeSelf);
            GameUI.instance.creditsTransform.gameObject.SetActive(!menuImg.activeSelf);
        }
        if (menuImg.activeSelf)
        {
            if (isInLanguageSelection)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    index--;
                    isInLanguageSelection = false;
                    return;
                }
                else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && (int)LocalizationManager.instance.currentLanguage < LocalizationManager.languageCount)
                {
                    LocalizationManager.instance.currentLanguage++;
                }
                else if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && (int)LocalizationManager.instance.currentLanguage > 0)
                {
                    LocalizationManager.instance.currentLanguage--;
                }
            }
            else
            {
                if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && index != 0)
                {
                    buttons[index].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                    index--;
                    buttons[index].GetComponent<Image>().color = selectedButtonColor;
                }
                else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && index != 3)
                {
                    buttons[index].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                    index++;

                    isInLanguageSelection = (index == 3);
                    if (isInLanguageSelection) return;

                    buttons[index].GetComponent<Image>().color = selectedButtonColor;
                }
                else if (Input.GetKeyDown(KeyCode.Return))
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
    }
    
    private void OnLevelSelectionPress()
    {
        GameUI.instance.levelSelectionTransform.gameObject.SetActive(true);
    }

    private void Credits()
    {
        GameUI.instance.creditsTransform.gameObject.SetActive(true);
    }

    private void Exit()
    {
        Application.Quit();
    }
}
