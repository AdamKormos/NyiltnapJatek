using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class Timer : MonoBehaviour
{
    public int sec { get; private set; }
    public static bool isPaused = false;

    public void OnGameLevelOpen()
    {
        StopCoroutine("Count");
        sec = 0;
        GameNS::StaticData.gameUI.timerText.text = "00:00";
        StartCoroutine("Count");
    }

    public IEnumerator Count()
    {
        while(!Player.reachedEnd)
        {
            if (!isPaused)
            {
                yield return new WaitForSeconds(1f);
                sec++;
                GameNS::StaticData.gameUI.timerText.text = ((sec % 60 < 10 ? "0" : "") + (int)(sec / 60) + ":" + (sec % 60 < 10 ? "0" : "") + (sec % 60)).ToString();
            }
            else yield return new WaitForSeconds(0.1f);
        }
    }
}
