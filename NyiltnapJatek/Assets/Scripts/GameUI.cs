using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class GameUI : MonoBehaviour
{
    [SerializeField] public Text timerText = default;
    [SerializeField] public Text levelCompletionPanelText = default;

    private void Start()
    {
        GameNS::StaticData.gameUI = this;
    }
}
