using UnityEngine;

/// <summary>
/// The class of an in-game coin.
/// TODO: Randomly generate coin value with a required amount given.
/// </summary>
public class Grade : MonoBehaviour
{ 
    public gradeEnum nem = default;

    private void Start()
    {
        transform.localScale = new Vector3(0.15f, 0.15f, 1);
        //GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GetComponent<CircleCollider2D>().radius = 2.65f;
        GetComponent<SpriteRenderer>().sprite = GameUI.instance.coinSprite;

        SetColorBasedOnGradeValue();
    }

    private void SetColorBasedOnGradeValue()
    {
        switch (nem)
        {
            case gradeEnum.one:
                GetComponent<SpriteRenderer>().color = new Color32(0, 175, 255, 255); // Sötétzöld
                break;
            case gradeEnum.two:
                GetComponent<SpriteRenderer>().color = new Color32(109, 109, 255, 255); // Szürkés
                break;
            case gradeEnum.three:
                GetComponent<SpriteRenderer>().color = new Color32(109, 0, 255, 255); // Lila
                break;
            case gradeEnum.four:
                GetComponent<SpriteRenderer>().color = new Color32(109, 255, 255, 255); // Világoszöld
                break;
            case gradeEnum.five:
                GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255); // Arany
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
