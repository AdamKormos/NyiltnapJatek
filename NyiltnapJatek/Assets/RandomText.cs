using UnityEngine;
using UnityEngine.UI;

public class RandomText : MonoBehaviour
{
    public static Color[] pointColors = new Color[5];
    public static Text txt = default;
    public static int pointCount = 0;
    public static int multiplier = 1;
    // Start is called before the first frame update
    void Start()
    {
        txt = transform.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
