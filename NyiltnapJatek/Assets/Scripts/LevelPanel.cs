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

    public void InjectRecord(string result, gradeAllSum.gradeEnum grade)
    {
        bestResultsText.text = result;
        gradeImage.sprite = LevelSelection.s_gradeSprites[(int)grade - 1];
    }
}
