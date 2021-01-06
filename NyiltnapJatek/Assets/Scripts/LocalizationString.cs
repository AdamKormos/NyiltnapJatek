using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalizationString
{
    [SerializeField] string[] texts = new string[2];

    public string current
    {
        get { return texts[(int)LocalizationManager.s_currentLanguage]; }
    }
}
