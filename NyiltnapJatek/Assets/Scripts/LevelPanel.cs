using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    [SerializeField] public Image levelImage, gradeImage;
    [SerializeField] public Text panelNameText, bestResultsText;

    public void PasteData(LevelPanelData lpd)
    {
        panelNameText.text = lpd.panelName;
        levelImage.sprite = lpd.levelImage;
    }

    public void InjectRecord(Tuple<string, gradeAllSum.gradeEnum> t)
    {
        if (t != null)
        {
            bestResultsText.text = t.Item1;
            gradeImage.sprite = LevelSelection.s_gradeSprites[(int)t.Item2 - 1];
        }
        else
        {
            gradeImage.sprite = LevelSelection.s_missingGradeSprite;
        }
    }
}
