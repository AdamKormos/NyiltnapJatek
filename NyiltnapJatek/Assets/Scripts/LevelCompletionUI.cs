using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class LevelCompletionUI : MonoBehaviour
{
    public void CallPanel()
    {
        GameNS::StaticData.gameUI.levelCompletionPanelText.text =
            GameNS::StaticData.gameUI.timerText.text + '\n' +
            "";
    }
}
