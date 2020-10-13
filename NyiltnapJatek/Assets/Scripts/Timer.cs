using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class Timer : MonoBehaviour
{
    public int sec { get; private set; }
    public int tenth { get; private set; }
    public static bool isPaused = false;

    public void OnGameLevelOpen()
    {
        StopCoroutine("Count");
        tenth = 0;
        sec = 0;
        GameNS::StaticData.gameUI.timerText.text = "00:00.0";
        StartCoroutine("Count");
    }

    public IEnumerator Count()
    {
        while(!Player.reachedEnd)
        {
            if (!isPaused)
            {
                yield return new WaitForSeconds(0.1f);
                tenth++;
                sec = tenth / 10;
                GameNS::StaticData.gameUI.timerText.text = ((sec % 60 < 10 ? "0" : "") + (int)(sec / 60) + ":" + (sec % 60 < 10 ? "0" : "") + (sec % 60) + "." + (tenth % 10)).ToString();
            }
            else yield return new WaitForSeconds(0.1f);
        }
    }
}
