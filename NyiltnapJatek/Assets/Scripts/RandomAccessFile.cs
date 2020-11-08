using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameNS = GameNS;

public class RandomAccessFile
{
    public static Tuple<string, gradeAllSum.gradeEnum> LoadData(int levelIndex)
    {
        string[] data = PlayerPrefs.GetString(levelIndex.ToString()).Split(' ');

        if (data.Length == 2)
        {
            return new Tuple<string, gradeAllSum.gradeEnum>(data[0], (gradeAllSum.gradeEnum)System.Convert.ToInt32(data[1]));
        }
        else return null;
    }

    public static void SaveData(int levelIndex, Tuple<string, gradeAllSum.gradeEnum> data)
    {
        PlayerPrefs.SetInt("MSI", LevelSelection.maxSceneIndex);
        PlayerPrefs.SetString(levelIndex.ToString(), data.Item1 + " " + ((int)data.Item2).ToString());
        PlayerPrefs.Save();
        GameNS::StaticData.gameUI.StartCoroutine(GameUI.UploadAverage());
    }

    public static float LoadAverage()
    {
        float sum = 0f;
        int correctAmount = 0;

        for(int levelIndex = 0; levelIndex <= 5; levelIndex++)
        {
            if(PlayerPrefs.GetFloat("FGrade" + levelIndex, 100f) != 100f)
            {
                sum += PlayerPrefs.GetFloat("FGrade" + levelIndex);
                correctAmount++;
            }
        }

        if (correctAmount == 0) sum = 1f;
        else sum /= (float)correctAmount;

        return Mathf.Clamp(sum, 1.00f, 5.00f);
    }

    public static void EraseData()
    {
        PlayerPrefs.DeleteAll();
    }
}