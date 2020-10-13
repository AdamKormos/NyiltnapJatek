using UnityEngine;

public class String : MonoBehaviour
{
    [SerializeField] private KeyCode key = default;
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (Input.GetKey(key))
    //    {
    //        RandomText.pointCount += collision.GetComponent<StringNote>().point * RandomText.multiplier;

    //        RandomText.multiplier *= 2;
    //        Destroy(collision.gameObject);
    //    }    
    //}



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(key))
        {
            RandomText.pointCount += collision.GetComponent<StringNote>().point * RandomText.multiplier;
            RandomText.multiplier *= 2;
            RandomText.
            Destroy(collision.gameObject);
        }
         
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RandomText.multiplier = 1;
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 4f), ForceMode2D.Impulse);
        RandomText.txt.text = RandomText.pointCount.ToString();
        Destroy(collision.gameObject);
    }
}
