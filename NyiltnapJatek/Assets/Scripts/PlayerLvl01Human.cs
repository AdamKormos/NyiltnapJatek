using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class PlayerLvl01Human : Player
{
    [SerializeField] protected float fallStrength = 0.05f;
    [SerializeField] protected float jumpStrength = 3;
    [SerializeField] protected float wingHealth = 100f;
    [SerializeField] protected float wingHealthIncreaseOnWaxPickup = 25f;
    [SerializeField] protected float wingHealthDecreasePerFrame = 0.05f;

    public static float s_wingHealth { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
#else
        moveStrength = 60;
        fallStrength = 0.04f;
        jumpStrength = 0.06f;
        wingHealthDecreasePerFrame = 0.2f;
        wingHealthIncreaseOnWaxPickup = 40;
#endif

        s_wingHealth = wingHealth;
        GameNS::StaticData.gameUI.leftTopSlider.maxValue = wingHealth;
        GameNS::StaticData.gameUI.leftTopSlider.value = GameNS::StaticData.gameUI.leftTopSlider.maxValue;

        cameraOffset = Camera.main.transform.position - transform.position;
        halfPlayerSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x / 2, GetComponent<SpriteRenderer>().bounds.size.y / 2);
        StartCoroutine(Move());
    }

    RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        if (!reachedEnd && !quizCollider.quizActive && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf)
        {
            if (Input.GetKey(KeyCode.Space) && GameNS::StaticData.gameUI.leftTopSlider.value > 0)
            {
                transform.position += new Vector3(0, jumpStrength);
            }
            else
            {
                transform.position += new Vector3(0, -fallStrength);
            }

            hit = Physics2D.Raycast(transform.position + new Vector3(0, halfPlayerSize.y + 1f), Vector2.up);

            if (hit.transform == null || !hit.transform.tag.Equals("Cloud"))
            {
                GameNS::StaticData.gameUI.leftTopSlider.value -= wingHealthDecreasePerFrame;
            }
        }

        currentPosition = transform.position;
    }

    Vector3 positionToAddOnFrame;

    protected override IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameNS::StaticData.gameUI.scoreCountText.GetComponent<Score>().OnGameLevelOpen();

        GameNS::StaticData.gameUI.leftTopSlider.gameObject.SetActive(true);

        while (!reachedEnd)
        {
            if (moveAllowed)
            {
                positionToAddOnFrame = new Vector3(0.05f * moveStrength, 0f) * Time.deltaTime
                    * (0.7f + ((GameNS::StaticData.gameUI.leftTopSlider.value / GameNS::StaticData.gameUI.leftTopSlider.maxValue / 10) * 3));
                transform.position += positionToAddOnFrame;
                Camera.main.transform.position += positionToAddOnFrame;
                yield return new WaitForEndOfFrame();
            }
            else yield return new WaitForSeconds(0.1f);
        }

        while (isOnScreen)
        {
            transform.position += new Vector3(0.05f * moveStrength, 0f) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("LevelEnding")) { reachedEnd = true; }
        else if (collision.tag.Equals("Wax"))
        {
            GameNS::StaticData.gameUI.leftTopSlider.value = Mathf.Clamp(GameNS::StaticData.gameUI.leftTopSlider.value + wingHealthIncreaseOnWaxPickup, 0, GameNS::StaticData.gameUI.leftTopSlider.maxValue);
            Destroy(collision.gameObject);
        }
        else if (collision.tag.Equals("PassiveEnemy") && moveAllowed) OnGameOver();
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
            if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize)
            {
                OnGameOver();
            }
            else if (transform.position.y < Camera.main.transform.position.y + Camera.main.orthographicSize && reachedEnd)
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
