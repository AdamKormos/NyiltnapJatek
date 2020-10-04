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
        sec = 0;
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        while(!Player.reachedEnd)
        {
            if (!isPaused)
            {
                sec++;
                GameNS::StaticData.gameUI.timerText.text = ((int)(sec / 60) + ":" + (sec % 60 < 10 ? "0" : "") + (sec % 60)).ToString();
                yield return new WaitForSeconds(1f);
            }
            else yield return new WaitForSeconds(0.1f);
        }
    }
}
