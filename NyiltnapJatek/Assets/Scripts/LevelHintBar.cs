using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The UI that appears at the beginning of the levels. Requires interaction to make it disappear and to start the gameplay.
/// </summary>
public class LevelHintBar : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) OnOk();
    }

    /// <summary>
    /// Called when the player pressed "OK" (hit enter). Allows the player to start the level.
    /// </summary>
    void OnOk()
    {
        this.gameObject.SetActive(false);
    }
}
