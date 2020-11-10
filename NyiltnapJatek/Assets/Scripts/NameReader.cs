using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameReader : MonoBehaviour
{
    public static bool allowedToGo { get; private set; }
    InputField infi;
    public static string textStr = "";

    // Start is called before the first frame update
    void Start()
    {
        infi = GetComponent<InputField>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.Return)) textStr = infi.text;
        Debug.Log(textStr);
        allowedToGo = (infi.text.Length > 0 && Input.GetKey(KeyCode.Return));
    }
}
