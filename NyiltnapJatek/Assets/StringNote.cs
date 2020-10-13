using UnityEngine;

public class StringNote : MonoBehaviour
{
    private Rigidbody2D rbd = default;
    public int point = 0;

    private void Start()
    {
        switch (point)
        {
            //pointes ugyek
        }
        rbd = transform.GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        rbd.AddForce(new Vector2(0f, -10f * Time.deltaTime), ForceMode2D.Force);
    }
}
