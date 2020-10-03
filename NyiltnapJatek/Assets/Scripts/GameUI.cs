using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class GameUI : MonoBehaviour
{
    [SerializeField] public Transform mainMenuTransform = default;
    [SerializeField] public Transform gameplayStuffTransform = default;
    [SerializeField] public Transform loadingScreenTransform = default;
    [SerializeField] public Text timerText = default;
    [SerializeField] public Text levelCompletionPanelText = default;
    [SerializeField] public Slider loadingScreenSlider = default;
    [SerializeField] bool startsInMainMenu = true;

    public void OnViewChanged(bool isMainMenuView)
    {
        if(isMainMenuView)
        {
            gameplayStuffTransform.gameObject.SetActive(false);
            mainMenuTransform.gameObject.SetActive(true);
        }
        else
        {
            gameplayStuffTransform.gameObject.SetActive(true);
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

    public static void ToggleChildren(GameObject parent, bool activityState)
    {
        RectTransform[] temp = parent.GetComponentsInChildren<RectTransform>(true);
        for (int i = 1; i < temp.Length; i++)
        {
            temp[i].gameObject.SetActive(activityState);
        }
    }
}
