using UnityEngine;

/// <summary>
/// Player child for Lvl04. Ability to toggle gravity and boost speed.
/// </summary>
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
        reachedEnd = false;

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
        if (collision.CompareTag("LevelEnding")) { reachedEnd = true; }
        else if (collision.CompareTag("DNASQ")) 
        { 
            moveStrength = Mathf.Clamp((int)(moveStrength * 1.25f), 0, 250);
            body.gravityScale = Mathf.Clamp(body.gravityScale * 1.5f, -10, 10f);
            Destroy(collision.gameObject);
        }
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

                //if (GameUI.instance.levelCompletionPanelParent != null)
                //{
                    LevelSelection.OnLevelCompleted();
                //}
            }
        }
    }
}
