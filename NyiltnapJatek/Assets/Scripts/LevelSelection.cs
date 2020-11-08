using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameNS = GameNS;

[System.Serializable]
public struct LevelPanelData
{
    [SerializeField] public string panelName;
    [SerializeField] public Sprite levelImage;
}

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
    static Tuple<int, gradeAllSum.gradeEnum>[] results = new Tuple<int, gradeAllSum.gradeEnum>[6];
    static string[] scoreRepresentations = new string[6];
    static LevelPanel[] panelChildren = default;
    static Image[] lockImageArray = new Image[5];

    private void OnEnable()
    {
        if(GameNS::StaticData.gameUI != null) GameNS::StaticData.gameUI.levelSelectionGuideText.gameObject.SetActive(true);

        if (panelChildren != null)
        {
            for (int i = 1; i < panelChildren.Length; i++) // panelChildren[0] is the sample panel
            {
                panelChildren[i].InjectRecord(RandomAccessFile.LoadData(i-1));
                panelChildren[i].gameObject.SetActive(true);

                if (i - 1 <= maxSceneIndex) lockImageArray[i - 1].enabled = false;
                else lockImageArray[i - 1].enabled = true;
            }
        }
    }

    private void OnDisable()
    {
        transform.position += new Vector3(currentSceneIndex * xOffsetBetweenPanels, 0);
        GameNS::StaticData.gameUI.levelSelectionGuideText.transform.position -= new Vector3(currentSceneIndex * xOffsetBetweenPanels, 0);
        currentSceneIndex = 0;

        GameNS::StaticData.gameUI.levelSelectionGuideText.gameObject.SetActive(false);
    }

    private void Start()
    {
#if UNITY_EDITOR
        maxSceneIndex = 4;
#else
        if(Debug.isDebugBuild) maxSceneIndex = 4;
        else maxSceneIndex = PlayerPrefs.GetInt("MSI", 0);
#endif

        s_missingGradeSprite = missingGradeSprite;
        s_gradeSprites = gradeSprites;
        samplePanel.SetActive(false);

        panelStartPosition = samplePanel.transform.position;
        guideTextStartPos = GameNS::StaticData.gameUI.levelSelectionGuideText.transform.position;

        if(panelChildren == null) InitiateChildrenCreation();
    }

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

        //Destroy(lockImageArray[0].gameObject); // First level is always available
        panelChildren = GetComponentsInChildren<LevelPanel>(true); // Collect panels in children
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentSceneIndex > 0)
        {
            currentSceneIndex--;
            transform.position += new Vector3(xOffsetBetweenPanels, 0);
            GameNS::StaticData.gameUI.levelSelectionGuideText.transform.position -= new Vector3(xOffsetBetweenPanels, 0);
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentSceneIndex < maxSceneIndex)
        {
            currentSceneIndex++;
            transform.position -= new Vector3(xOffsetBetweenPanels, 0);
            GameNS::StaticData.gameUI.levelSelectionGuideText.transform.position += new Vector3(xOffsetBetweenPanels, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            GameNS::StaticData.loadingScreen.LoadLevel(currentSceneIndex+1);
        }
    }

    public static void FetchCompletionData(int resultScore, gradeAllSum.gradeEnum resultGrade)
    {
        int arrIndex = SceneManager.GetActiveScene().buildIndex; // Because the sample panel is part of the array too

        Tuple<int, gradeAllSum.gradeEnum> data;

        var loadedData = RandomAccessFile.LoadData(arrIndex - 1);

        if (loadedData == null)
        {
            if(arrIndex == 5) data = new Tuple<int, gradeAllSum.gradeEnum>(0, gradeAllSum.gradeEnum.one);
            else data = new Tuple<int, gradeAllSum.gradeEnum>(Int32.MaxValue, gradeAllSum.gradeEnum.one);
        }
        else
        {
            data = new Tuple<int, gradeAllSum.gradeEnum>(PlayerPrefs.GetInt("LvlRes" + arrIndex, arrIndex == 5 ? 0 : Int32.MaxValue), loadedData.Item2);
        }

        string resultScoreString = "";
        int dataScore = data.Item1;
        gradeAllSum.gradeEnum dataGrade = data.Item2;

        if (arrIndex == 5)
        {
            if (resultScore >= data.Item1)
            {
                resultScoreString = GameNS::StaticData.gameUI.scoreCountText.text;
                Debug.Log(GameNS::StaticData.gameUI.scoreCountText.text);
            }
            else resultScoreString = loadedData.Item1;
        }
        else
        {
            if (resultScore <= data.Item1)
            {
                resultScoreString = GameNS::StaticData.gameUI.scoreCountText.text;
                PlayerPrefs.SetInt("LvlRes" + arrIndex, resultScore);
            }
            else resultScoreString = loadedData.Item1;
        }
        
        if (resultGrade > dataGrade) dataGrade = resultGrade;

        RandomAccessFile.SaveData(arrIndex - 1, new Tuple<string, gradeAllSum.gradeEnum>(resultScoreString, dataGrade));
    }

    public static void OnLevelCompleted()
    {
        if(maxSceneIndex < 4) maxSceneIndex = Mathf.Clamp(currentSceneIndex + 1, 0, 4);

        PlayerPrefs.SetInt("MSI", maxSceneIndex);
        PlayerPrefs.Save();
        lockImageArray[maxSceneIndex].enabled = false;

        GameNS::StaticData.gameUI.levelCompletionPanelParent.CallPanel(true);
    }
}
