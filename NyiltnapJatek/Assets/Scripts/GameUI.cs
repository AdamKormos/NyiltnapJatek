using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
#pragma warning disable UNT0013
    //[SerializeField] public Text debugText = default;
    [SerializeField] public InputField nameInputField = default;
    [SerializeField] public Sprite coinSprite = default;
    [SerializeField] public Transform mainMenuTransform = default;
    [SerializeField] public Transform creditsTransform = default;
    [SerializeField] public Transform gameplayStuffTransform = default;
    [SerializeField] public Transform loadingScreenTransform = default;
    [SerializeField] public Transform levelSelectionTransform = default;
    [SerializeField] public Transform quizTransform = default;
    [SerializeField] public Text correctAnswerText = default;
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
    [SerializeField] public GameObject lvl05HealthIndicatorSliderObject = default;
    [SerializeField] public Text levelSelectionGuideText = default;
    [SerializeField] public Text keyGuide = default;
    [SerializeField] bool startsInMainMenu = true; // A field used for debugging and testing. By default, this wouldn't be needed for the released version.
#pragma warning restore UNT0013
    public static bool loads = false;
    public static GameUI instance = default;

    private void Awake()
    {
        // Storing the instance. This if-else is necessary because for the sake of testing, GameUI objects are placed on every level but there should be 
        // only one at the same time.
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            instance.OnViewChanged(startsInMainMenu, false);
        }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        if(Application.isPlaying) StartCoroutine(StartRoutine());
    }

    /// <summary>
    /// Uploads a user's most recent results to the database. Called when a level is finished.
    /// </summary>
    /// <returns></returns>
    public static IEnumerator UploadAverage()
    {
        WWWForm form = new WWWForm();
        form.AddField("nev", PlayerPrefs.GetString("Username"));
        float result = RandomAccessFile.LoadAverage();
        form.AddField("atlag", result.ToString());

        UnityWebRequest www = UnityWebRequest.Post("neumanngame.atwebpages.com/index.php", form);
        yield return www.SendWebRequest(); // Formerly www.Send();
    }

    /// <summary>
    /// Called when the game starts and is responsible for checking the user's username and further game init actions such as cursor visibility and loading the UI based
    /// on the selected language.
    /// </summary>
    /// <returns></returns>
    IEnumerator StartRoutine()
    {
        //#if UNITY_EDITOR
        //        PlayerPrefs.SetString("Username", "");
        //#endif

        if (startsInMainMenu)
        {
            RefreshLanguageOnAllLocalizationTextsInsideUI();

            if (PlayerPrefs.GetString("Username", "") == "")
            {
                mainMenuTransform.gameObject.SetActive(false);
                nameInputField.gameObject.SetActive(true);
                nameInputField.interactable = true;
                nameInputField.Select();

                while (nameInputField.text.Length == 0 || !Input.GetKey(KeyCode.Return))
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                mainMenuTransform.gameObject.SetActive(true);
                nameInputField.gameObject.SetActive(false);
            }

            PlayerPrefs.SetString("Username", nameInputField.text);
            PlayerPrefs.Save();

            mainMenuTransform.gameObject.SetActive(true);
        }
        nameInputField.gameObject.SetActive(false);

        //StartCoroutine(UploadName());
        StartCoroutine(TriggerLevelSelectionChildrenCreation());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0 && !quizCollider.quizActive && !instance.levelHintBar.gameObject.activeSelf && LoadingScreen.finishedLoading)
            {
                if (Input.GetKeyDown(KeyCode.Escape) && !LoadingScreen.startedLoading)
                {
                    levelSelectionGuideText.gameObject.SetActive(false);
                    LoadingScreen.finishedLoading = false;
                    LoadingScreen.instance.LoadLevel(0);
                }
                else if (Input.GetKeyDown(KeyCode.R) && LoadingScreen.finishedLoading) // OnGameOver() copy paste
                {
                    loads = true;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    instance.OnViewChanged(false, true);
                }
            }
        }
        //if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.U)) RandomAccessFile.EraseData();
    }

    /// <summary>
    /// Makes the level selection transform active for one frame. By doing so, its Start function will execute and the level selection panel's menus will be created.
    /// </summary>
    /// <returns></returns>
    IEnumerator TriggerLevelSelectionChildrenCreation()
    {
        levelSelectionTransform.gameObject.SetActive(true);
        yield return new WaitForSeconds(1 / 60);
        levelSelectionTransform.gameObject.SetActive(false);
    }

    /// <summary>
    /// Loads a hint that appears at the beginning of the levels. The hint can become hidden by pressing enter after activating it. Once the hint became hidden, the
    /// gameplay may start.
    /// </summary>
    /// <param name="levelHintString"></param>
    public void LoadLevelHint(string levelHintString)
    {
        levelHintBar.gameObject.SetActive(true);
        levelHintBarText.text = levelHintString;
    }

    /// <summary>
    /// Toggles a gameobject's transform children.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="activityState"></param>
    public static void ToggleChildren(GameObject parent, bool activityState)
    {
        RectTransform[] temp = parent.GetComponentsInChildren<RectTransform>(true);
        for (int i = 1; i < temp.Length; i++)
        {
            temp[i].gameObject.SetActive(activityState);
        }
    }

    /// <summary>
    /// Called when a scene transmission happens.
    /// </summary>
    /// <param name="isMainMenuView"></param>
    /// <param name="isReloadingLevel"></param>
    public void OnViewChanged(bool isMainMenuView, bool isReloadingLevel)
    {
        lvl05StuffTransform.gameObject.SetActive(false);
        leftTopSlider.gameObject.SetActive(false);
        levelSelectionGuideText.gameObject.SetActive(false);
        keyGuide.gameObject.SetActive(false);
        scoreCountText.gameObject.SetActive(false);
        correctAnswerText.gameObject.SetActive(false);
        quizTransform.gameObject.SetActive(false);
        creditsTransform.gameObject.SetActive(false);
        levelSelectionTransform.gameObject.SetActive(false);

        mainMenuTransform.gameObject.SetActive(isMainMenuView);
        gameplayStuffTransform.gameObject.SetActive(!isMainMenuView);

        if (isMainMenuView)
        {
            ToggleChildren(mainMenuTransform.gameObject, true);
        }
        else
        {
            gradeAllSum.sum = 0;
            quizMaxAll.correctQuestions = 0;
            Score.value = 0;
            Quiz.checkpoint = null;

            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 5:
                    instance.scoreCountText.text = "0";
                    break;
                default:
                    instance.scoreCountText.text = "00:00.0";
                    break;
            }

            if (!isReloadingLevel)
            {
                switch (SceneManager.GetActiveScene().buildIndex)
                {
                    case 1:
                        instance.LoadLevelHint("A mitológiából ismert Ikarosszal kell végigrepülnöd a pályán. Hogy túléld az utat, szükséged lesz a pályán elszórt viaszokra, amik megelőzik, hogy elolvadjon a szárnyad! A szárny nem olvad ha felhő alatt vagy. A szóközzel tudsz repülni. A szárny \"életét\" bal felül találod.");
                        break;
                    case 2:
                        instance.LoadLevelHint("Törd szét az összes téglát a golyó segítségével! Minden tégla két ütés után törik szét. Vigyázz, ha lezuhan a golyó, újra kell kezdened a pályát. A platformot vízszintesen tudod mozgatni.");
                        break;
                    case 3:
                        instance.LoadLevelHint("Mozogj a kottavonalakon (fel: W/fel nyíl, le: S/le nyíl), hogy kikerüld az akadályokat! A vonalak közé is lemehetsz, ha lenyomva tartod az Alt-ot. Az Alt elengedésével visszakerülsz arra a vonalra, amelyikről leereszkedtél.");
                        break;
                    case 4:
                        instance.LoadLevelHint("Menj végig a pályán használva a gravitáció változtatgatását a szóközzel! A DNS szekvenciák felvételével gyorsabban mész. Vigyázz, mert ha túl sokáig blokkolja valami az utad, lemaradsz, és akkor újra kell kezdened a pályát." +
                            "\n" + "\n" + "Megjegyzés: Minden pályán lévő tárgy akadály és nekik tudsz ütközni (azaz semmi nem tartozik a háttérhez).");
                        break;
                    case 5:
                        instance.LoadLevelHint("Védd meg a szervereket az ellenfelek elpusztításával! A szóközzel tudsz lőni, az űrhajót pedig vízszintesen tudod mozgatni. A szerver életét és a lövedéked mennyiségét bal felül láthatod. A pályán lövedékcsomagon is vannak elszórva, melyek 5 és 20 közötti mennyiségű lövedéket adnak.");
                        break;
                }
            }

            levelCompletionPanelText.transform.parent.gameObject.SetActive(true);
            ToggleChildren(levelCompletionPanelText.transform.parent.gameObject, false);
        }
    }

    /// <summary>
    /// Called to load the UI texts using the correct language.
    /// </summary>
    public static void RefreshLanguageOnAllLocalizationTextsInsideUI()
    {
        foreach(Text text in instance.GetComponentsInChildren<Text>())
        {
            LocalizationText localizationText = text.gameObject.AddComponent<LocalizationText>();
            localizationText = (LocalizationText)text;
            localizationText.SetHunText(text.text);
            localizationText.RefreshText();
            DestroyImmediate(text);
        }
    }
}
