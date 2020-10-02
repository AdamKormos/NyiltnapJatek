using UnityEngine;

public enum gradeEnum { one = 1, two, three, four, five };

public class compactGrade : MonoBehaviour
{
    GameObject coin;
    public gradeEnum nem;
}


public class Grade : MonoBehaviour
{
    [SerializeField] private compactGrade[] gradeArray = new compactGrade[3]; 
    public static int count = 0;
    public static int osszegzes_tetele = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
