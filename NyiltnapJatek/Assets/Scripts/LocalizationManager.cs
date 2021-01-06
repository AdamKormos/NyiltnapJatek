using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocalizationLanguage { Hungarian, English };

public class LocalizationManager : MonoBehaviour
{
    [SerializeField] LocalizationLanguage currentLanguage = LocalizationLanguage.Hungarian;
    public static LocalizationLanguage s_currentLanguage;

    // Start is called before the first frame update
    void Start()
    {
        s_currentLanguage = currentLanguage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
