using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Used for storing information about a level panel. It contains a name and an image. These can be edited in the inspector directly and provide a clean way to work with
/// level panels in different ways.
/// </summary>
[System.Serializable]
public struct LevelPanelData
{
    [SerializeField] public string panelName;
    [SerializeField] public Sprite levelImage;
}

/// <summary>
/// The class for the level selection menu.
/// TODO: Has optimization potential and tons of junk to remove.
/// </summary>
public class LevelSelection : MonoBehaviour
{
    [SerializeField] GameObject lockGameObject = default;
    [SerializeField] Sprite[] gradeSprites = new Sprite[5];
    [SerializeField] Sprite missingGradeSprite = default;
    [SerializeField] float xOffsetBetweenPanels = 60f;
    [SerializeField] GameObject samplePanel = default;
    [SerializeField] List<LevelPanelData> levelPanels = new List<LevelPanelData>();
    Vector2 panelStartPosition = default, guideTextStartPos = default;
    public static int currentSceneIndex = 0;
    public static int maxSceneIndex { get; private set; }
    public static Sprite[] s_gradeSprites { get; private set; }
    public static Sprite s_missingGradeSprite { get; private set; }
    static bool[] completedLevel = new bool[5];
    static Tuple<int, gradeEnum>[] results = new Tuple<int, gradeEnum>[6];
    static string[] scoreRepresentations = new string[6];
    static LevelPanel[] panelChildren = default;
    static Image[] lockImageArray = new Image[5];

    /// <summary>
    /// Displays guide text and level panels if the array is already initialized. If so, it (for now) excludes the first element (the sample panel, potentially can be removed)
    /// and loads data for each panel if possible.
    /// </summary>
    private void OnEnable()
    {
        if(GameUI.instance != null) GameUI.instance.levelSelectionGuideText.gameObject.SetActive(true); // Guide text appears

        if (panelChildren != null)
        {
            for (int i = 1; i < panelChildren.Length; i++) // panelChildren[0] is the sample panel
            {
                panelChildren[i].InjectRecord(RandomAccessFile.LoadData(i-1));
                panelChildren[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Removes the applied camera offset so that it points to the first panel when the player opens the menu for the first time. Sets the guide text inactive.
    /// </summary>
    private void OnDisable()
    {
        transform.position += new Vector3(currentSceneIndex * xOffsetBetweenPanels, 0);
        GameUI.instance.levelSelectionGuideText.transform.position -= new Vector3(currentSceneIndex * xOffsetBetweenPanels, 0);
        currentSceneIndex = 0;

        GameUI.instance.levelSelectionGuideText.gameObject.SetActive(false);
    }

    private void Start()
    {
#if UNITY_EDITOR
        maxSceneIndex = PlayerPrefs.GetInt("MSI", 0);
#else
        maxSceneIndex = PlayerPrefs.GetInt("MSI", 0);
#endif

        s_missingGradeSprite = missingGradeSprite;
        s_gradeSprites = gradeSprites;
        samplePanel.SetActive(false);

        panelStartPosition = samplePanel.transform.position;
        guideTextStartPos = GameUI.instance.levelSelectionGuideText.transform.position;

        if(panelChildren == null) InitiateChildrenCreation();
    }

    /// <summary>
    /// Main method for creating the level panel objects. Applies the needed offset for each instantiated panel and spawns a lock if the player isn't allowed to progress
    /// further.
    /// </summary>
    private void InitiateChildrenCreation()
    {
        for (int i = 0; i < levelPanels.Count; i++)
        {
            GameObject panel = Instantiate(samplePanel, transform.position + new Vector3(i * xOffsetBetweenPanels, 0), Quaternion.identity, this.transform);
            GameObject lockObject = Instantiate(lockGameObject, transform.position + new Vector3(i * xOffsetBetweenPanels, 0) + lockGameObject.transform.position,
                                              Quaternion.identity, panel.transform);
            lockImageArray[i] = lockObject.GetComponent<Image>();

            LevelPanelData toPaste = levelPanels[i];
            panel.GetComponent<LevelPanel>().PasteData(toPaste);

            if (i <= maxSceneIndex) lockImageArray[i].enabled = false;
        }

        panelChildren = GetComponentsInChildren<LevelPanel>(true); // Collect panels in children
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentSceneIndex > 0)
        {
            currentSceneIndex--;
            transform.position += new Vector3(xOffsetBetweenPanels, 0);
            GameUI.instance.levelSelectionGuideText.transform.position -= new Vector3(xOffsetBetweenPanels, 0);
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentSceneIndex < maxSceneIndex)
        {
            currentSceneIndex++;
            transform.position -= new Vector3(xOffsetBetweenPanels, 0);
            GameUI.instance.levelSelectionGuideText.transform.position += new Vector3(xOffsetBetweenPanels, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            recentOpenedLevel = currentSceneIndex;
            LoadingScreen.instance.LoadLevel(recentOpenedLevel + 1);
        }
        else if (Input.GetKeyDown(KeyCode.T)) UnlockAllLevels();
    }

    public static int recentOpenedLevel = 0;

    /// <summary>
    /// Called when a level is completed. Determines which data to save out of the existing and the new ones. Technically speaking, it handles the player's best results.
    /// </summary>
    /// <param name="resultScore"></param>
    /// <param name="resultGrade"></param>
    public static void FetchCompletionData(int resultScore, gradeEnum resultGrade)
    {
        int arrIndex = SceneManager.GetActiveScene().buildIndex; // Because the sample panel is part of the array too and main menu is index 0.

        Tuple<int, gradeEnum> data;

        Tuple<string, gradeEnum> loadedData = RandomAccessFile.LoadData(arrIndex - 1);

        if (loadedData == null)
        {
            if(arrIndex == 5) data = new Tuple<int, gradeEnum>(0, gradeEnum.one);
            else data = new Tuple<int, gradeEnum>(Int32.MaxValue, gradeEnum.one);
        }
        else
        {
            data = new Tuple<int, gradeEnum>(PlayerPrefs.GetInt("LvlRes" + arrIndex, arrIndex == 5 ? 0 : Int32.MaxValue), loadedData.Item2);
        }

        string resultScoreString = "";
        int dataScore = data.Item1;
        gradeEnum dataGrade = data.Item2;

        if (resultGrade >= dataGrade)
        {
            dataGrade = resultGrade;

            if (arrIndex == 5)
            {
                if (resultScore >= data.Item1)
                {
                    resultScoreString = GameUI.instance.scoreCountText.text;
                    Debug.Log(GameUI.instance.scoreCountText.text);
                }
                else resultScoreString = loadedData.Item1;
            }
            else
            {
                if (resultScore <= data.Item1)
                {
                    resultScoreString = GameUI.instance.scoreCountText.text;
                    PlayerPrefs.SetInt("LvlRes" + arrIndex, resultScore);
                    PlayerPrefs.Save();
                }
                else resultScoreString = loadedData.Item1;
            }
        }
        else resultScoreString = (loadedData.Item1 == "" || loadedData.Item1 == null ? GameUI.instance.scoreCountText.text : loadedData.Item1);

        RandomAccessFile.SaveData(arrIndex - 1, 
            new Tuple<string, gradeEnum>(resultScoreString == "" ? GameUI.instance.scoreCountText.text : resultScoreString, dataGrade));
    }

    /// <summary>
    /// Called by player on level completion. Handles level progression and removes locks if needed.
    /// </summary>
    public static void OnLevelCompleted()
    {
        if (maxSceneIndex != 4)
        {
            if (recentOpenedLevel + 1 > maxSceneIndex) maxSceneIndex = Mathf.Clamp(recentOpenedLevel + 1, 0, 4);

            PlayerPrefs.SetInt("MSI", maxSceneIndex);
            PlayerPrefs.Save();

            for (int i = 0; i <= maxSceneIndex; i++)
            {
                lockImageArray[i].enabled = false;
            }

            for (int i = maxSceneIndex + 1; i < 5; i++)
            {
                lockImageArray[i].enabled = true;
            }
        }

        GameUI.instance.levelCompletionPanelParent.CallPanel(true);
    }

    public static void UnlockAllLevels()
    {
        maxSceneIndex = 4;
        PlayerPrefs.SetInt("MSI", maxSceneIndex);
        PlayerPrefs.Save();

        for (int i = 0; i <= maxSceneIndex; i++)
        {
            lockImageArray[i].enabled = false;
        }
        GameUI.instance.levelCompletionPanelParent.CallPanel(true);
    }
}
