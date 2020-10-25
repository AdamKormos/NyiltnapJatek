using UnityEngine;
using GameNS = GameNS;

public class PlayerLvl4Biosz : Player
{
    private Rigidbody2D body = default;
    private GameObject arrow = default;

    [SerializeField] private float rotationScale = -20f;
    [SerializeField] private float velocity = 20f;


    bool canRotate = true;
    bool checkCollision = false;
    //bool upsidedown = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        arrow = transform.GetComponentsInChildren<Transform>()[1].gameObject;

        levelCompletionPanelParent = GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.GetComponent<LevelCompletionUI>();
        if (levelCompletionPanelParent != null) levelCompletionPanelParent.CallPanel(false);

        StartCoroutine(Move());
    }

    void Update()
    {
        if (canRotate)
        {
            if (transform.rotation.eulerAngles.z >= 90.0f && transform.rotation.eulerAngles.z <= 270)
            {
                rotationScale = -rotationScale;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                //body.AddRelativeForce(new Vector2(transform.right.x, 0), ForceMode2D.Force);
                canRotate = false;
                arrow.SetActive(false);
                //body.AddForce(transform.right * velocity);
                //body.AddForceAtPosition(new Vector2(velocity, velocity), transform.position);
                body.AddRelativeForce(new Vector2(velocity, 0f), ForceMode2D.Impulse);
                checkCollision = true;
            }
            transform.Rotate(Vector3.forward * rotationScale * Time.deltaTime, Space.Self);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("LevelEnding")) { reachedEnd = true; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (checkCollision)
        {
            body.velocity = new Vector2(0f, 0f);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            arrow.SetActive(true);
            canRotate = true;
            checkCollision = false;
        }
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;

        if (Camera.main != null)
        {
            if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize)
            {
                OnGameOver();
            }
            else if (transform.position.y < Camera.main.transform.position.y + Camera.main.orthographicSize)
            {
                if (levelCompletionPanelParent != null)
                {
                    LevelSelection.OnLevelCompleted();
                    levelCompletionPanelParent.CallPanel(true);
                }
            }
        }
    }
}
