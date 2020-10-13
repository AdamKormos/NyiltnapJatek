using UnityEngine;

public class gradeAllSum : MonoBehaviour
{
    public static int count = 0;
    public static int sum = 0;
    public static int maxSum = 0;
    public enum gradeEnum { one = 1, two, three, four, five };

    private void Start()
    {
        Grade[] grades = FindObjectsOfType<Grade>();

        for(int i = 0; i < grades.Length; i++)
        {
            maxSum += (int)grades[i].nem;
        }
    }
}
