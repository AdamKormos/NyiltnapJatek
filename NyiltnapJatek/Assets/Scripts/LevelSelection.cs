using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] Sprite[] gradeSprites = new Sprite[5];
    [SerializeField] float xOffsetBetweenPanels = 60f;
    [SerializeField] GameObject samplePanel = default;
    [SerializeField] List<LevelPanelData> levelPanels = new List<LevelPanelData>();
    Vector2 panelStartPosition = default;
    public static int currentIndex = 0;
    public static int maxIndex = 0;
    public static Menu.Scenes currentScene { get; private set; }
    static bool[] completedLevel = new bool[5];
    static Tuple<int, gradeAllSum.gradeEnum>[] results = new Tuple<int, gradeAllSum.gradeEnum>[5];
    static string[] scoreRepresentations = new string[5];
    static LevelPanel[] panelChildren = default;
    public static Sprite[] s_gradeSprites { get; private set; }

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
        transform.position += new Vector3(currentIndex * xOffsetBetweenPanels, 0);
        currentIndex = 0;
    }

    private void Start()
    {
#if UNITY_EDITOR
        maxIndex = 4;
#else
        maxIndex = 0;
#endif
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

            LevelPanelData toPaste = levelPanels[i];
            panel.GetComponent<LevelPanel>().PasteData(toPaste);
        }

        panelChildren = GetComponentsInChildren<LevelPanel>(true);
    }

    private void Update()
    {
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.Space)) maxIndex = 4;

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentIndex < maxIndex)
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex++;
            }
            else
            {
                transform.position += new Vector3(xOffsetBetweenPanels, 0);
            }
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentIndex < maxIndex)
        {
            currentIndex++;
            if (currentIndex > levelPanels.Count-1)
            {
                currentIndex--;
            }
            else
            {
                transform.position -= new Vector3(xOffsetBetweenPanels, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            currentScene = (Menu.Scenes)currentIndex+1;
            GameNS::StaticData.loadingScreen.LoadLevel(currentScene);
        }
    }

    public static void FetchCompletionData(int resultScore, gradeAllSum.gradeEnum resultGrade)
    {
        int arrIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; // Because the sample panel is part of the array too

        int newTupleInt;
        gradeAllSum.gradeEnum newTupleGrade;

        if (results[arrIndex] == null) results[arrIndex] = new Tuple<int, gradeAllSum.gradeEnum>(Int32.MaxValue, gradeAllSum.gradeEnum.one);

        if (arrIndex == 1 || arrIndex == 2 || arrIndex == 4)
        {
            if (resultScore < results[arrIndex].Item1)
            {
                newTupleInt = resultScore;
                scoreRepresentations[arrIndex] = GameNS::StaticData.gameUI.scoreCountText.text;
            }
            else newTupleInt = results[arrIndex].Item1;
        }
        else
        {
            if (resultScore > results[arrIndex].Item1)
            {
                newTupleInt = resultScore;
                scoreRepresentations[arrIndex] = GameNS::StaticData.gameUI.scoreCountText.text;
                Debug.Log(GameNS::StaticData.gameUI.scoreCountText.text);
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
        if (currentScene == Menu.Scenes.mainMenu) return;

        if (!completedLevel[(int)currentScene - 1])
        {
            completedLevel[(int)currentScene - 1] = true;
            maxIndex++;
        }
    }
}
