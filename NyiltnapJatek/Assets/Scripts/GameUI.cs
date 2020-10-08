﻿using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public Text timerText = default;
    [SerializeField] public Text levelCompletionPanelText = default;
    [SerializeField] public Slider loadingScreenSlider = default;
    [SerializeField] bool startsInMainMenu = true;
#pragma warning restore UNT0013

    public void OnViewChanged(bool isMainMenuView)
    {
        if(isMainMenuView)
        {
            gameplayStuffTransform.gameObject.SetActive(false);
            levelSelectionTransform.gameObject.SetActive(false);
            quizTransform.gameObject.SetActive(false);
            mainMenuTransform.gameObject.SetActive(true);
            ToggleChildren(mainMenuTransform.gameObject, true);
        }
        else
        {
            gameplayStuffTransform.gameObject.SetActive(true);

            levelCompletionPanelText.transform.parent.gameObject.SetActive(true);
            ToggleChildren(levelCompletionPanelText.transform.parent.gameObject, false);

            timerText.gameObject.SetActive(true);

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
            GameNS::StaticData.gameUI.OnViewChanged(startsInMainMenu);
        }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
