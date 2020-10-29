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
    Vector2 panelStartPosition = default;
    public static int currentSceneIndex = 0;
    public static int maxSceneIndex { get; private set; }
    public static Sprite[] s_gradeSprites { get; private set; }
    public static Sprite s_missingGradeSprite { get; private set; }
    static bool[] completedLevel = new bool[5];
    static Tuple<int, gradeAllSum.gradeEnum>[] results = new Tuple<int, gradeAllSum.gradeEnum>[5];
    static string[] scoreRepresentations = new string[5];
    static LevelPanel[] panelChildren = default;
    static Image[] lockImageArray = new Image[5];

    private void OnEnable()
    {
        if (panelChildren != null)
        {
            for (int i = 1; i < panelChildren.Length; i++) // panelChildren[0] is the sample panel
            {
                panelChildren[i].InjectRecord(RandomAccessFile.LoadData(i-1));
                panelChildren[i].gameObject.SetActive(true);
            }
        }
    }

    private void OnDisable()
    {
        transform.position += new Vector3(currentSceneIndex * xOffsetBetweenPanels, 0);
        currentSceneIndex = 0;
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

        if(panelChildren == null) InitiateChildrenCreation();
    }

    private void InitiateChildrenCreation()
    {
        for (int i = 0; i < levelPanels.Count; i++)
        {
            GameObject panel = Instantiate(samplePanel, transform.position + new Vector3(i * xOffsetBetweenPanels, 0), Quaternion.identity, this.transform);
            GameObject lockObject = Instantiate(lockGameObject, transform.position + new Vector3(i * xOffsetBetweenPanels, 0) + lockGameObject.transform.position, 
                                                Quaternion.identity, panel.transform);

            LevelPanelData toPaste = levelPanels[i];
            panel.GetComponent<LevelPanel>().PasteData(toPaste);

            lockImageArray[i] = lockObject.GetComponent<Image>();
        }

        Destroy(lockImageArray[0].gameObject); // First level is always available
        panelChildren = GetComponentsInChildren<LevelPanel>(true); // Collect panels in children
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentSceneIndex > 0)
        {
            currentSceneIndex--;
            transform.position += new Vector3(xOffsetBetweenPanels, 0);
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentSceneIndex < maxSceneIndex)
        {
            currentSceneIndex++;
            transform.position -= new Vector3(xOffsetBetweenPanels, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            GameNS::StaticData.loadingScreen.LoadLevel(currentSceneIndex+1);
        }
    }

    public static void FetchCompletionData(int resultScore, gradeAllSum.gradeEnum resultGrade)
    {
        int arrIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; // Because the sample panel is part of the array too
        
        int newTupleInt;
        gradeAllSum.gradeEnum newTupleGrade;

        if (results[arrIndex] == null)
        {
            if(arrIndex == 5) results[arrIndex] = new Tuple<int, gradeAllSum.gradeEnum>(0, gradeAllSum.gradeEnum.one);
            else results[arrIndex] = new Tuple<int, gradeAllSum.gradeEnum>(Int32.MaxValue, gradeAllSum.gradeEnum.one);
        }

        if (arrIndex == 5)
        {
            if (resultScore > results[arrIndex].Item1)
            {
                newTupleInt = resultScore;
                scoreRepresentations[arrIndex] = GameNS::StaticData.gameUI.scoreCountText.text;
                Debug.Log(GameNS::StaticData.gameUI.scoreCountText.text);
            }
            else newTupleInt = results[arrIndex].Item1;
        }
        else
        {
            if (resultScore < results[arrIndex].Item1)
            {
                newTupleInt = resultScore;
                scoreRepresentations[arrIndex] = GameNS::StaticData.gameUI.scoreCountText.text;
            }
            else newTupleInt = results[arrIndex].Item1;
        }
        
        if (resultGrade > results[arrIndex].Item2) newTupleGrade = resultGrade;
        else newTupleGrade = results[arrIndex].Item2;

        results[arrIndex] = new Tuple<int, gradeAllSum.gradeEnum>(newTupleInt, newTupleGrade);

        RandomAccessFile.SaveData(arrIndex - 1, new Tuple<string, gradeAllSum.gradeEnum>(scoreRepresentations[arrIndex], results[arrIndex].Item2));
    }

    public static void OnLevelCompleted()
    {
        if (currentSceneIndex != 0)
        {
            if (!completedLevel[currentSceneIndex - 1])
            {
                completedLevel[currentSceneIndex - 1] = true;
                maxSceneIndex++;
                Destroy(lockImageArray[maxSceneIndex].gameObject);
            }
        }

        GameNS::StaticData.gameUI.levelCompletionPanelParent.CallPanel(true);
    }
}
