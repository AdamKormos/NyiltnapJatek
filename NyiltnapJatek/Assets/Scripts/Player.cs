using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

public class Player : MonoBehaviour
{
    [SerializeField] protected LevelCompletionUI levelCompletionPanelParent = default;
    [SerializeField] protected int movePerSec = 20;
    [SerializeField] protected int moveStrength = 1;
    public static bool moveAllowed = true;
    [SerializeField] protected float jumpStrength = 3;
    protected bool isOnGround = true;
    protected float halfPlayerSize = 0f;
    protected Vector3 cameraStartPos, startPos;
    public static bool reachedEnd { get; protected set; }
    public static bool isOnScreen { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        levelCompletionPanelParent = GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.GetComponent<LevelCompletionUI>();
        if(levelCompletionPanelParent != null) levelCompletionPanelParent.CallPanel(false);
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
        }
    }

    protected virtual IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameNS::StaticData.gameUI.scoreCountText.GetComponent<Score>().OnGameLevelOpen(LevelSelection.currentScene);

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

    protected virtual void OnGameOver()
    {
        Score.isPaused = false;
        GameNS::StaticData.gameUI.quizTransform.gameObject.SetActive(false);
        quizCollider.quizActive = false;
        Player.moveAllowed = true;

        transform.position = startPos;
        Camera.main.transform.position = cameraStartPos;
        startPos = transform.position;
        cameraStartPos = Camera.main.transform.position;

        GameNS::StaticData.gameUI.scoreCountText.GetComponent<Score>().OnGameLevelOpen(LevelSelection.currentScene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("LevelEnding")) { reachedEnd = true; }
        isOnGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isOnGround = false;
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;
        reachedEnd = false; // Setting it back to false for further levels

        if (levelCompletionPanelParent != null)
        {
            levelCompletionPanelParent.CallPanel(true);
        }
    }
}
