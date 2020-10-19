using UnityEngine;

public class BallLvl02MatekFizika : MonoBehaviour
{
    Rigidbody2D rbd = default;
    private float force = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        rbd = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Vector2 vel = -(new Vector2(rbd.velocity.x, rbd.velocity.y));
        //rbd.velocity = new Vector2(0f, 0f);
        //rbd.AddForce(vel * force);
        rbd.AddForce(-rbd.velocity.normalized * force, ForceMode2D.Force);
    }
}
