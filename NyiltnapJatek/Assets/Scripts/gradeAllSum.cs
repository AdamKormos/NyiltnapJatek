using UnityEngine;

public enum gradeEnum { one = 1, two, three, four, five };

/// <summary>
/// A class responsible for holding player result related values.
/// TODO: See if any Unity objects have this class and if it's used for something related to MonoBehaviour stuff. If not, make this class static.
/// </summary>
public class gradeAllSum : MonoBehaviour
{
    public static int count = 0;
    public static int sum = 0;
    public static int maxSum = 0;
}
