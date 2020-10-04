using UnityEngine;

public enum gradeEnum { one = 1, two, three, four, five };

public class Grade : MonoBehaviour
{ 
    public GameObject coin = default;
    public gradeEnum nem = default;
    public static int count = 0;
    public static int sum = 0;
    
    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>())
        {
            Grade.count++;
            Grade.sum += (int)nem;
        }
        Destroy(this.gameObject);
    }


}


public class gradeWrapper
{
    public Grade[] gradeArr = new Grade[3];
}
