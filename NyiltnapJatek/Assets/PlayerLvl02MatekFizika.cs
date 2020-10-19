using UnityEngine;

public class PlayerLvl02MatekFizika : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(10f * Time.deltaTime, 0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(10f * Time.deltaTime, 0f);
        }
    }
}
