using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class PlayerLvl01Human : Player
{
    [SerializeField] protected float fallStrength = 0.05f;
    [SerializeField] protected string cloudObjectTag = "Cloud";
    [SerializeField] protected GameObject wingHealthSliderGameObject = default;
    [SerializeField] protected float wingHealth = 100f;
    [SerializeField] protected float wingHealthIncreaseOnWaxPickup = 25f;
    [SerializeField] protected float wingHealthDecreasePerFrame = 0.05f;
    Slider wingHealthSlider = default;

    // Start is called before the first frame update
    void Start()
    {
        try { halfPlayerSize = GetComponent<BoxCollider2D>().size.y / 2; }
        catch { halfPlayerSize = GetComponent<CircleCollider2D>().radius; }

        wingHealthSliderGameObject.SetActive(false);

        wingHealthSliderGameObject = Instantiate(wingHealthSliderGameObject.gameObject, wingHealthSliderGameObject.transform.position 
                                                                    + new Vector3(GameNS::StaticData.gameUI.GetComponent<RectTransform>().rect.width / 2,
                                                                                  GameNS::StaticData.gameUI.GetComponent<RectTransform>().rect.height / 2),
                                                                                  Quaternion.identity, GameNS::StaticData.gameUI.transform);
        wingHealthSlider = wingHealthSliderGameObject.GetComponent<Slider>();
        wingHealthSlider.maxValue = wingHealth;
        wingHealthSlider.value = wingHealthSlider.maxValue;

        startPos = transform.position;
        cameraStartPos = Camera.main.transform.position;

        levelCompletionPanelParent = GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.GetComponent<LevelCompletionUI>();
        if (levelCompletionPanelParent != null) levelCompletionPanelParent.CallPanel(false);

        GameNS::StaticData.gameUI.LoadLevelHint("Repülj végig a pályán! Hogy túléld az utat, szükséged lesz a pályán elszórt viaszokra, amik megelőzik, hogy elolvadjon a szárnyad!");
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

            hit = Physics2D.Raycast(transform.position + new Vector3(0, halfPlayerSize + 1f), Vector2.up);

            if (hit.transform == null || !hit.transform.tag.Equals(cloudObjectTag)) // TODO: Remove second condition?
            {
                wingHealthSlider.value -= wingHealthDecreasePerFrame;
            }
        }
    }

    protected override IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameNS::StaticData.gameUI.scoreCountText.GetComponent<Score>().OnGameLevelOpen(Menu.Scenes.Lvl1);

        // Level hint read, gameplay starts:
        wingHealthSliderGameObject.SetActive(true);

        while (!reachedEnd)
        {
            if (moveAllowed)
            {
                transform.position += new Vector3(0.05f * moveStrength, 0f);
                Camera.main.transform.position += new Vector3(0.05f * moveStrength, 0f);
                yield return new WaitForSeconds(1f / movePerSec);
            }
            else yield return new WaitForSeconds(0.1f);
        }

        while (isOnScreen)
        {
            transform.position += new Vector3(0.05f * moveStrength, 0f);
            yield return new WaitForSeconds(1f / movePerSec);
        }
    }

    protected override void OnGameOver()
    {
        base.OnGameOver();

        wingHealthSlider.maxValue = wingHealth;
        wingHealthSlider.value = wingHealthSlider.maxValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("LevelEnding")) { reachedEnd = true; }
        else if (collision.tag.Equals("Wax"))
        {
            wingHealthSlider.value = Mathf.Clamp(wingHealthSlider.value + wingHealthIncreaseOnWaxPickup, 0, wingHealthSlider.maxValue);
            Destroy(collision.gameObject);
        }
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

                if (levelCompletionPanelParent != null)
                {
                    LevelSelection.OnLevelCompleted();
                    levelCompletionPanelParent.CallPanel(true);
                }
            }
        }
    }
}
