using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class GameUI : MonoBehaviour
{
#pragma warning disable UNT0013
    [SerializeField] public Transform mainMenuTransform = default;
    [SerializeField] public Transform gameplayStuffTransform = default;
    [SerializeField] public Transform loadingScreenTransform = default;
    [SerializeField] public Transform levelSelectionTransform = default;
    [SerializeField] public Transform quizTransform = default;
    [SerializeField] public Transform lvl05StuffTransform = default;
    [SerializeField] public Text bulletCountText = default;
    [SerializeField] public Text scoreCountText = default;
    [SerializeField] public Text levelCompletionPanelText = default;
    [SerializeField] public Slider loadingScreenSlider = default;
    [SerializeField] public LevelHintBar levelHintBar = default;
    [SerializeField] public Text levelHintBarText = default;
    [SerializeField] bool startsInMainMenu = true;
#pragma warning restore UNT0013

    public void OnViewChanged(bool isMainMenuView, bool isReloadingLevel)
    {
        // Set level specific objects to false:
        lvl05StuffTransform.gameObject.SetActive(false);

        quizTransform.gameObject.SetActive(false);

        if (isMainMenuView)
        {
            gameplayStuffTransform.gameObject.SetActive(false);
            levelSelectionTransform.gameObject.SetActive(false);
            mainMenuTransform.gameObject.SetActive(true);
            ToggleChildren(mainMenuTransform.gameObject, true);
        }
        else
        {
            if (!isReloadingLevel)
            {
                switch (LevelSelection.currentScene)
                {
                    case Menu.Scenes.Lvl1:
                        GameNS::StaticData.gameUI.LoadLevelHint("Repülj végig a pályán! Hogy túléld az utat, szükséged lesz a pályán elszórt viaszokra, amik megelőzik, hogy elolvadjon a szárnyad!");
                        break;
                    case Menu.Scenes.Lvl2:
                        GameNS::StaticData.gameUI.LoadLevelHint("Flappy Bird - Repülj végig a pályán és ugorj a szóköz segítségével!");
                        break;
                    case Menu.Scenes.Lvl3:
                        break;
                    case Menu.Scenes.Lvl4:
                        break;
                    case Menu.Scenes.Lvl5:
                        GameNS::StaticData.gameUI.LoadLevelHint("Védd meg a szervereket az ellenfelek elpusztításával! A szóközzel tudsz lőni.");
                        break;
                }
            }

            gameplayStuffTransform.gameObject.SetActive(true);

            levelCompletionPanelText.transform.parent.gameObject.SetActive(true);
            ToggleChildren(levelCompletionPanelText.transform.parent.gameObject, false);

            scoreCountText.gameObject.SetActive(true);

            levelSelectionTransform.gameObject.SetActive(false);
            mainMenuTransform.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        if (GameNS::StaticData.gameUI == null)
        {
            GameNS::StaticData.gameUI = this;
            DontDestroyOnLoad(this.gameObject);
            GameNS::StaticData.gameUI.OnViewChanged(startsInMainMenu, false);
        }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadLevelHint(string levelHintString)
    {
        levelHintBar.gameObject.SetActive(true);
        levelHintBarText.text = levelHintString;
    }

    public static void ToggleChildren(GameObject parent, bool activityState)
    {
        RectTransform[] temp = parent.GetComponentsInChildren<RectTransform>(true);
        for (int i = 1; i < temp.Length; i++)
        {
            temp[i].gameObject.SetActive(activityState);
        }
    }
}
