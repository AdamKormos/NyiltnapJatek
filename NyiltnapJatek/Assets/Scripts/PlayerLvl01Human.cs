using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class PlayerLvl01Human : Player
{
    [SerializeField] protected float fallStrength = 0.05f;
    [SerializeField] protected float jumpStrength = 3;
    [SerializeField] protected GameObject wingHealthSliderGameObject = default;
    [SerializeField] protected float wingHealth = 100f;
    [SerializeField] protected float wingHealthIncreaseOnWaxPickup = 25f;
    [SerializeField] protected float wingHealthDecreasePerFrame = 0.05f;
    Slider wingHealthSlider = default;

    // Start is called before the first frame update
    void Start()
    {
        halfPlayerSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x / 2, GetComponent<SpriteRenderer>().bounds.size.y / 2);

        wingHealthSliderGameObject.SetActive(false);

        wingHealthSliderGameObject = Instantiate(wingHealthSliderGameObject.gameObject, wingHealthSliderGameObject.transform.position 
                                                                    + new Vector3(GameNS::StaticData.gameUI.GetComponent<RectTransform>().rect.width / 2,
                                                                                  GameNS::StaticData.gameUI.GetComponent<RectTransform>().rect.height / 2),
                                                                                  Quaternion.identity, GameNS::StaticData.gameUI.transform);
        wingHealthSlider = wingHealthSliderGameObject.GetComponent<Slider>();
        wingHealthSlider.maxValue = wingHealth;
        wingHealthSlider.value = wingHealthSlider.maxValue;

        StartCoroutine(Move());
    }

    RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        if (!reachedEnd && !quizCollider.quizActive && !GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf)
        {
            if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && wingHealthSlider.value > 0)
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
                wingHealthSlider.value -= wingHealthDecreasePerFrame;
            }
        }
    }

    Vector3 positionToAddOnFrame;

    protected override IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameNS::StaticData.gameUI.scoreCountText.GetComponent<Score>().OnGameLevelOpen();

        wingHealthSliderGameObject.SetActive(true);

        while (!reachedEnd)
        {
            if (moveAllowed)
            {
                positionToAddOnFrame = new Vector3(0.05f * moveStrength, 0f) * Time.deltaTime * (0.7f + ((wingHealthSlider.value / wingHealthSlider.maxValue / 10) * 3));
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

    protected override void OnGameOver()
    {
        base.OnGameOver();

        Destroy(wingHealthSliderGameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("LevelEnding")) { reachedEnd = true; }
        else if (collision.tag.Equals("Wax"))
        {
            wingHealthSlider.value = Mathf.Clamp(wingHealthSlider.value + wingHealthIncreaseOnWaxPickup, 0, wingHealthSlider.maxValue);
            Destroy(collision.gameObject);
        }
        else if (collision.tag.Equals("PassiveEnemy")) OnGameOver();
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
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
                reachedEnd = false; // Setting it back to false for further levels

                Destroy(wingHealthSliderGameObject);

                if (GameNS::StaticData.gameUI.levelCompletionPanelParent != null)
                {
                    LevelSelection.OnLevelCompleted();
                }
            }
        }
    }
}
