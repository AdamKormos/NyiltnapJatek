using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public static void EraseData()
    {
        PlayerPrefs.DeleteAll();
    }
}