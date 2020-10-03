using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class LevelCompletionUI : MonoBehaviour
{
    private void Start()
    {
        GetComponentInChildren<Button>(true).onClick.AddListener(GameNS::StaticData.loadingScreen.LoadToMainMenu);
    }

    public void CallPanel(bool activityState)
    {
        GameUI.ToggleChildren(this.gameObject, activityState);

        GameNS::StaticData.gameUI.levelCompletionPanelText.text =
            GameNS::StaticData.gameUI.timerText.text + '\n' +
            "";
    }
}
