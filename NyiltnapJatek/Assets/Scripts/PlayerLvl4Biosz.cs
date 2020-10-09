using UnityEngine;

public class PlayerLvl4Biosz : MonoBehaviour
{
    private Rigidbody2D body = default;
    [SerializeField] private float velocity = 0.5f;
    

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity = -velocity;
            body.velocity = new Vector2(0f, velocity);
        }
        
    }
}
