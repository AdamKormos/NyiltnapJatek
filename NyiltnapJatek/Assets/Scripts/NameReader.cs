using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameReader : MonoBehaviour
{
    public static bool allowedToGo { get; private set; }
    InputField infi;

    // Start is called before the first frame update
    void Start()
    {
        infi = GetComponent<InputField>();    
    }

    // Update is called once per frame
    void Update()
    {
        allowedToGo = (infi.text.Length > 0 && Input.GetKey(KeyCode.Return));
    }
}
