using System.Collections;
using UnityEngine;

public class PlayerLvl03Muveszetek : MonoBehaviour
{
    [SerializeField] Rigidbody2D rbd = default;
    [SerializeField] private int index = 0; 
    [SerializeField] private float kottaGap = 8f;
    [SerializeField] private float waitSecond = 12f;

    private float i = 0f;

    private bool isMozog = false;
    private bool altUp = false;

    // Start is called before the first frame update
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        waitSecond = 1f / waitSecond;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            altUp = true;
            isMozog = false;
            StartCoroutine(move((float)index - i));
        }
        else if (Input.GetKey(KeyCode.W) && index != 5)
        {
            StartCoroutine(move(kottaGap));
        }
        else if (Input.GetKey(KeyCode.S) && index != 0)
        {
            StartCoroutine(move(-kottaGap));
        }

    }

    IEnumerator move(float num)
    {
        if (!isMozog)
        {
            num /= Input.GetKey(KeyCode.LeftAlt) ? 2 : 1;
            altUp = false;

            isMozog = true;
            for (i = 0f; i < num && !altUp; i += 0.5f)
            {
                transform.position += new Vector3(num, 0f);
                yield return new WaitForSeconds(waitSecond);
            }
            isMozog = false;
        }
        yield return new WaitForSeconds(0.05f);
    }
}
