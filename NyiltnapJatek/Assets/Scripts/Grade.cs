using UnityEngine;
using GameNS = GameNS;

public class Grade : MonoBehaviour
{ 
    public gradeAllSum.gradeEnum nem = default;

    private void Start()
    {
        transform.localScale = new Vector3(0.15f, 0.15f, 1);
        //GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GetComponent<SpriteRenderer>().sprite = GameNS::StaticData.gameUI.coinSprite;

        switch(nem)
        {
            case gradeAllSum.gradeEnum.one:
                GetComponent<SpriteRenderer>().color = new Color32(0, 175, 255, 255);
                break;
            case gradeAllSum.gradeEnum.two:
                GetComponent<SpriteRenderer>().color = new Color32(109, 109, 255, 255);
                break;
            case gradeAllSum.gradeEnum.three:
                GetComponent<SpriteRenderer>().color = new Color32(109, 0, 255, 255);
                break;
            case gradeAllSum.gradeEnum.four:
                GetComponent<SpriteRenderer>().color = new Color32(109, 255, 255, 255);
                break;
            case gradeAllSum.gradeEnum.five:
                GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                break;
        }
    }

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
