using System.Collections;
using UnityEngine;

public class BallLvl02MatekFizika : MonoBehaviour
{
    Rigidbody2D rbd = default;
    float lowerYBound;
    [SerializeField] Vector2 velocity = new Vector2(0, 10f);

    private void OnDisable()
    {
        rbd.Sleep();
    }

    private void OnEnable()
    {
        if(rbd != null) rbd.WakeUp();
    }

    // Start is called before the first frame update
    void Start()
    {
        lowerYBound = Camera.main.transform.position.y - Camera.main.orthographicSize;
        rbd = transform.GetComponent<Rigidbody2D>();
        rbd.AddForce(new Vector3(transform.right.x * velocity.y, transform.up.y * velocity.y), ForceMode2D.Impulse);
        velocity = rbd.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        rbd.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<ObstacleLvl02>() || (collision.gameObject.GetComponent<ObstacleLvl02>() && !collision.gameObject.GetComponent<ObstacleLvl02>().IsQuestionAndDestroyed()))
        {
            velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
        }

        velocity.x = (velocity.x < 0 ? Mathf.Clamp(velocity.x, Mathf.NegativeInfinity, -1f) : Mathf.Clamp(velocity.x, 1f, Mathf.Infinity));
        velocity.y = (velocity.y < 0 ? Mathf.Clamp(velocity.y, Mathf.NegativeInfinity, -1f) : Mathf.Clamp(velocity.y, 1f, Mathf.Infinity));
    }

    private void OnBecameInvisible()
    {
        if (!GameUI.loads)
        {
            PlayerLvl02MatFiz.isBallOnScreen = false;
        }
    }
}
