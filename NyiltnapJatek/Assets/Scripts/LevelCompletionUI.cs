using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class LevelCompletionUI : MonoBehaviour
{
    public void CallPanel(bool activityState)
    {
        RectTransform[] temp = GetComponentsInChildren<RectTransform>(true);
        for (int i = 1; i < temp.Length; i++)
        {
            temp[i].gameObject.SetActive(activityState);
        }

        GameNS::StaticData.gameUI.levelCompletionPanelText.text =
            GameNS::StaticData.gameUI.timerText.text + '\n' +
            "";
    }
}
