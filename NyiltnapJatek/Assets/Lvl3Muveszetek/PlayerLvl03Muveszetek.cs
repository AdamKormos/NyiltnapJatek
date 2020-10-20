using System.Collections;
using UnityEngine;

public class PlayerLvl03Muveszetek : MonoBehaviour
{
    [SerializeField] Rigidbody2D rbd = default;
    [SerializeField] private int index = 0; 
    [SerializeField] private float kottaGap = 8f;
    [SerializeField] private float waitSecond = 12f;

    private bool isMozog = false;

    // Start is called before the first frame update
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        waitSecond = 1f / waitSecond;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                StartCoroutine(move(0.5f, true));
            }
            else
            {
                StartCoroutine(move(0.5f, false));
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                StartCoroutine(move(-0.5f, true));
            }
            else
            {
                StartCoroutine(move(-0.5f, false));
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            StartCoroutine(move(transform.position.x / kottaGap > index ? -0.5f : 0.5f, false));
        }

    }

    IEnumerator move(float num, bool alt)
    {
        if (!isMozog)
        {
            index += (int)(2 * num);
            isMozog = true;
            for (int i = 0; i < (alt ? kottaGap / 2 : kottaGap); kottaGap += 0.5f)
            {
                transform.position += new Vector3(num, 0f);
                yield return new WaitForSeconds(0.1f);
            }
            isMozog = false;

        }
        yield return new WaitForSeconds(0.05f);
    }
}
