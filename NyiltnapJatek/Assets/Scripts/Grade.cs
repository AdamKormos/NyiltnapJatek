using UnityEngine;

public class Grade : MonoBehaviour
{ 
    public gradeAllSum.gradeEnum nem = default;
    
    [System.Obsolete]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Player>())
        {
            gradeAllSum.count++;
            gradeAllSum.sum += (int)nem;
        }
        Destroy(this.gameObject);
    }


}
