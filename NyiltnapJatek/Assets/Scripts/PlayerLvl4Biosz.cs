using UnityEngine;
using GameNS = GameNS;

public class PlayerLvl4Biosz : Player
{
    private Rigidbody2D body = default;
    //bool upsideDown = false;

    void Start()
    {
#if UNITY_EDITOR
#else
        moveStrength = 60;
#endif

        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;

        cameraOffset = Camera.main.transform.position - transform.position;
        halfPlayerSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x / 2, GetComponent<SpriteRenderer>().bounds.size.y / 2);
        StartCoroutine(Move());
    }

    void Update()
    {
        currentPosition = transform.position;

        if (body.velocity.y == 0 && !reachedEnd && !quizCollider.quizActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                body.gravityScale = -body.gravityScale;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("LevelEnding")) { reachedEnd = true; }
        else if (collision.tag.Equals("DNASQ")) 
        { 
            moveStrength = (int)(moveStrength * 1.25f);
            body.gravityScale *= 1.5f;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (checkCollision)
        //{
        //    body.velocity = new Vector2(0f, 0f);
        //    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
        //    arrow.SetActive(true);
        //    canRotate = true;
        //    checkCollision = false;
        //}
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;

        if (respawnedAtCheckpoint)
        {
            respawnedAtCheckpoint = false;
        }
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;

        if (Camera.main != null)
        {
            if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize || transform.position.x < Camera.main.transform.position.x - ((2f * Camera.main.orthographicSize * Camera.main.aspect) / 2))
            {
                OnGameOver();
            }
            else if(reachedEnd)
            {
                reachedEnd = false;

                //if (GameNS::StaticData.gameUI.levelCompletionPanelParent != null)
                //{
                    LevelSelection.OnLevelCompleted();
                //}
            }
        }
    }
}
