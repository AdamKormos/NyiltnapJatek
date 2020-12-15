using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for saving and loading level results based on level indeces.
/// </summary>
public class RandomAccessFile
{
    /// <summary>
    /// Loads results based on a level index.
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <returns></returns>
    public static Tuple<string, gradeEnum> LoadData(int levelIndex)
    {
        string[] data = PlayerPrefs.GetString(levelIndex.ToString()).Split(' ');

        if (data.Length == 2)
        {
            return new Tuple<string, gradeEnum>(data[0], (gradeEnum)System.Convert.ToInt32(data[1]));
        }
        else return null;
    }

    /// <summary>
    /// Saves results to a location defined by the level index. Also uploads the current stats to the database.
    /// </summary>
    /// <param name="levelIndex"></param>
    /// <param name="data"></param>
    public static void SaveData(int levelIndex, Tuple<string, gradeEnum> data)
    {
        PlayerPrefs.SetInt("MSI", LevelSelection.maxSceneIndex);
        PlayerPrefs.SetString(levelIndex.ToString(), data.Item1 + " " + ((int)data.Item2).ToString());
        PlayerPrefs.Save();
        GameUI.instance.StartCoroutine(GameUI.UploadAverage());
    }

    /// <summary>
    /// Calculates the average of the completed levels.
    /// </summary>
    /// <returns></returns>
    public static float LoadAverage()
    {
        float sum = 0f;
        int correctAmount = 0;

        for(int levelIndex = 0; levelIndex < 5; levelIndex++)
        {
            if(PlayerPrefs.GetFloat("FGrade" + levelIndex, 100f) != 100f)
            {
                //Debug.Log(PlayerPrefs.GetFloat("FGrade" + levelIndex));
                sum += PlayerPrefs.GetFloat("FGrade" + levelIndex);
                correctAmount++;
            }
        }

        if (correctAmount == 0) sum = 1f;
        else sum /= (float)correctAmount;

        return Mathf.Clamp(sum, 1.00f, 5.00f);
    }

    /// <summary>
    /// Deletes every saved data.
    /// </summary>
    public static void EraseData()
    {
        PlayerPrefs.DeleteAll();
    }
}