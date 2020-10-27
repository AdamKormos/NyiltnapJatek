using UnityEngine;

public class Grade : MonoBehaviour
{ 
    public gradeAllSum.gradeEnum nem = default;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() || collision.GetComponent<Bullet>() || collision.GetComponent<BallLvl02MatekFizika>())
        {
            gradeAllSum.count++;
            gradeAllSum.sum += (int)nem;
            Destroy(this.gameObject);
        }
    }
}
