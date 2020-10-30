using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameNS = GameNS;

public class GameUI : MonoBehaviour
{
#pragma warning disable UNT0013
    //[SerializeField] public Text debugText = default;
    [SerializeField] public Sprite coinSprite = default;
    [SerializeField] public Transform mainMenuTransform = default;
    [SerializeField] public Transform creditsTransform = default;
    [SerializeField] public Transform gameplayStuffTransform = default;
    [SerializeField] public Transform loadingScreenTransform = default;
    [SerializeField] public Transform levelSelectionTransform = default;
    [SerializeField] public Transform quizTransform = default;
    [SerializeField] public Text quizQuestionText = default;
    [SerializeField] public Transform lvl05StuffTransform = default;
    [SerializeField] public Text bulletCountText = default;
    [SerializeField] public Text scoreCountText = default;
    [SerializeField] public Text levelCompletionPanelText = default;
    [SerializeField] public LevelCompletionUI levelCompletionPanelParent = default;
    [SerializeField] public Slider loadingScreenSlider = default;
    [SerializeField] public LevelHintBar levelHintBar = default;
    [SerializeField] public Text levelHintBarText = default;
    [SerializeField] public Slider leftTopSlider = default;
    [SerializeField] public Text levelSelectionGuideText = default;
    [SerializeField] bool startsInMainMenu = true;
#pragma warning restore UNT0013

    public void OnViewChanged(bool isMainMenuView, bool isReloadingLevel)
    {
        // Set level specific objects to false:
        lvl05StuffTransform.gameObject.SetActive(false);
        leftTopSlider.gameObject.SetActive(false);
        levelSelectionGuideText.gameObject.SetActive(false);

        quizTransform.gameObject.SetActive(false);

        if (isMainMenuView)
        {
            gameplayStuffTransform.gameObject.SetActive(false);
            creditsTransform.gameObject.SetActive(false);
            levelSelectionTransform.gameObject.SetActive(false);
            mainMenuTransform.gameObject.SetActive(true);
            ToggleChildren(mainMenuTransform.gameObject, true);
        }
        else
        {
            gradeAllSum.sum = 0;
            quizMaxAll.correctQuestions = 0;
            Quiz.checkpoint = null;

            if (!isReloadingLevel)
            {
                switch (SceneManager.GetActiveScene().buildIndex)
                {
                    case 1:
                        GameNS::StaticData.gameUI.LoadLevelHint("Repülj végig a pályán! Hogy túléld az utat, szükséged lesz a pályán elszórt viaszokra, amik megelőzik, hogy elolvadjon a szárnyad! A szóközzel tudsz repülni. A szárny \"életét\" bal felül találod.");
                        break;
                    case 2:
                        GameNS::StaticData.gameUI.LoadLevelHint("Törd szét az összes téglát a golyó segítségével! Vigyázz, ha lezuhan a golyó, újra kell kezdened a pályát. A platformot vízszintesen tudod mozgatni.");
                        break;
                    case 3:
                        GameNS::StaticData.gameUI.LoadLevelHint("Mozogj a kottavonalakon (fel: W/fel nyíl, le: S/le nyíl), hogy kikerüld az akadályokat! A vonalak közé is lemehetsz, ha lenyomva tartod az Alt-ot. Az Alt elengedésével visszakerülsz arra a vonalra, amelyikről leereszkedtél.");
                        break;
                    case 4:
                        GameNS::StaticData.gameUI.LoadLevelHint("Menj végig a pályán használva a gravitáció változtatgatását a szóközzel! A DNS szekvenciák felvételével gyorsabban mész. Vigyázz, mert ha túl sokáig blokkolja valami az utad, lemaradsz, és akkor újra kell kezdened a pályát." +
                            "\n" + "\n" + "Megjegyzés: Minden pályán lévő tárgy akadály és nekik tudsz ütközni (azaz semmi nem tartozik a háttérhez).");
                        break;
                    case 5:
                        GameNS::StaticData.gameUI.LoadLevelHint("Védd meg a szervereket az ellenfelek elpusztításával! A szóközzel tudsz lőni, az űrhajót pedig vízszintesen tudod mozgatni. A szerver életét és a lövedéked mennyiségét bal felül láthatod.");
                        break;
                }

                switch(SceneManager.GetActiveScene().buildIndex)
                {
                    case 5:
                        GameNS::StaticData.gameUI.scoreCountText.text = "0";
                        break;
                    default:
                        GameNS::StaticData.gameUI.scoreCountText.text = "00:00.0";
                        break;
                }
            }

            gameplayStuffTransform.gameObject.SetActive(true);

            levelCompletionPanelText.transform.parent.gameObject.SetActive(true);
            ToggleChildren(levelCompletionPanelText.transform.parent.gameObject, false);

            scoreCountText.gameObject.SetActive(true);

            levelSelectionTransform.gameObject.SetActive(false);
            mainMenuTransform.gameObject.SetActive(false);
            creditsTransform.gameObject.SetActive(false);
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
        StartCoroutine(GenerateLevelSelectionChildren());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0 && !quizCollider.quizActive && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf && LoadingScreen.finishedLoading)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                levelSelectionGuideText.gameObject.SetActive(false);
                GameNS::StaticData.loadingScreen.LoadLevel(0);
            }
            else if(Input.GetKeyDown(KeyCode.R)) // OnGameOver() copy paste
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                OnViewChanged(false, true);
            }
        }
    }

    IEnumerator GenerateLevelSelectionChildren()
    {
        levelSelectionTransform.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        levelSelectionTransform.gameObject.SetActive(false);
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
