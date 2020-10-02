using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text timerText = default;
    int sec = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        while(true)
        {
            sec++;
            timerText.text = ((int)(sec / 60) + ":" +  (sec % 60 < 10 ? "0" : "") + (sec % 60)).ToString();
            yield return new WaitForSeconds(1f);
        }
    }
}
