using UnityEngine;

public class ObstacleLvl02 : MonoBehaviour
{
    private int health = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ++health;
        switch (health)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}
