using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocalizationLanguage { Hungarian, English };

public class LocalizationManager : MonoBehaviour
{
    [SerializeField] public LocalizationLanguage currentLanguage = LocalizationLanguage.Hungarian;
    public static int languageCount = 2;
    public static LocalizationManager instance = default;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
}
