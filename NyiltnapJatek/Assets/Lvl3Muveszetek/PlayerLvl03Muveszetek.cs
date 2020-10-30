using System.Collections;
using System.Globalization;
using UnityEngine;
using GameNS = GameNS;

public class PlayerLvl03Muveszetek : Player
{
    [SerializeField] KeyCode halfMoveKey = default;
    [SerializeField] private float kottaGap = 8f;
    [SerializeField] private int moveTickAmount = 30;
    [SerializeField] private float waitSecond = 12f;
    private bool Moving = false;
    public static int index = 0;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
#else
        moveStrength = 60;
        moveTickAmount = 20;
#endif

        index = 0;
        heldAltAtStart = false;
        waitSecond = 1f / waitSecond;

        cameraOffset = Camera.main.transform.position - transform.position;

        cameraOffset = Camera.main.transform.position - transform.position;
        halfPlayerSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x / 2, GetComponent<SpriteRenderer>().bounds.size.y / 2);
        StartCoroutine(Move()); // Inherited, automatic Move
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;
        
        if(heldAltAtStart && quizCollider.quizActive) StartCoroutine(move(kottaGap / 2));
        else if (!Moving && !quizCollider.quizActive)
        {
            if (heldAltAtStart)
            {
                if (Input.GetKeyUp(halfMoveKey)) // Reached max distance with alt move (so move method won't detect this while descending)
                {
                    StartCoroutine(move(kottaGap / 2));
                }
            }
            else
            {
                if (Input.GetKeyDown(halfMoveKey))
                {
                    StartCoroutine(move(-kottaGap / 2));
                }
                else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && index != 4)
                {
                    index++;
                    StartCoroutine(move(kottaGap));
                }
                else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && index != 0)
                {
                    index--;
                    StartCoroutine(move(-kottaGap));
                }
            }
        }
    }

    //bool brokeAltMove = false;
    public static bool heldAltAtStart = false;

    IEnumerator move(float num)
    {
        Moving = true;
        heldAltAtStart = Input.GetKey(halfMoveKey) && !quizCollider.quizActive;

        for (int i = 0; i < moveTickAmount; i++)
        {
            if ((!Input.GetKey(halfMoveKey) || quizCollider.quizActive) && heldAltAtStart)
            {
                StartCoroutine(move((kottaGap / 2) / moveTickAmount * i));
                yield break;
            }

            transform.position += new Vector3(0, num / moveTickAmount);
            yield return new WaitForEndOfFrame();
        }

        Moving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("LevelEnding")) { reachedEnd = true; }
        else if (collision.tag.Equals("PassiveEnemy") && moveAllowed) OnGameOver();
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;

        if (respawnedAtCheckpoint)
        {
            heldAltAtStart = false;
            respawnedAtCheckpoint = false;
        }
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;

        if (Camera.main != null && reachedEnd)
        {
            reachedEnd = false;
            LevelSelection.OnLevelCompleted();
        }
    }
}
