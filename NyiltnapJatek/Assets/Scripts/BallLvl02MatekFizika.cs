using UnityEngine;

public class BallLvl02MatekFizika : MonoBehaviour
{
    Rigidbody2D rbd = default;

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
        //Vector2 vel = -(new Vector2(rbd.velocity.x, rbd.velocity.y));
        //rbd.velocity = new Vector2(0f, 0f);
        //rbd.AddForce(vel * force);
        //rbd.AddForce(-rbd.velocity.normalized * force, ForceMode2D.Force);
        velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
    }

    private void OnBecameInvisible()
    {
        PlayerLvl02MatFiz.isBallOnScreen = false;
    }
}
