using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A panel responsible for displaying level information in the level selection menu.
/// </summary>
public class LevelPanel : MonoBehaviour
{
    [SerializeField] public Image levelImage, gradeImage;
    [SerializeField] public Text panelNameText, bestResultsText;

    /// <summary>
    /// Puts a LevelPanelData's information into this level panel. Is used at panel creation as LevelPanelDatas have names and images.
    /// </summary>
    /// <param name="lpd"></param>
    public void PasteData(LevelPanelData lpd)
    {
        panelNameText.text = lpd.panelName;
        levelImage.sprite = lpd.levelImage;
    }

    /// <summary>
    /// Overwrites this panel's result information. If for any reason results related to this panel's level can't be loaded, it'll handle the level as uncompleted.
    /// </summary>
    /// <param name="t"></param>
    public void InjectRecord(Tuple<string, gradeEnum> t)
    {
        if (t != null)
        {
            bestResultsText.text = t.Item1;
            gradeImage.sprite = LevelSelection.s_gradeSprites[(int)t.Item2 - 1];
        }
        else
        {
            bestResultsText.text = ""; // <-- Shouldn't cause any bugs(?)
            gradeImage.sprite = LevelSelection.s_missingGradeSprite;
        }
    }
}
