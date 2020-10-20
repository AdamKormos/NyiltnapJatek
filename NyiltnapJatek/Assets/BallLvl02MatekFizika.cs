using UnityEngine;

public class BallLvl02MatekFizika : MonoBehaviour
{
    Rigidbody2D rbd = default;

    Vector3 velocity = new Vector3(10f, 0f);
    
    // Start is called before the first frame update
    void Start()
    {
        rbd = transform.GetComponent<Rigidbody2D>();
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
        velocity = Vector2.Reflect(-rbd.velocity, Vector2.right);
    }
}
