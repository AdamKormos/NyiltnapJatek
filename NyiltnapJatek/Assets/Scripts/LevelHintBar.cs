using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class LevelHintBar : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) OnOk();
    }

    void OnOk()
    {
        this.gameObject.SetActive(false);
    }
}
