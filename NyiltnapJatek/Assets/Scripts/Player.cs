using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameNS = GameNS;

public class Player : MonoBehaviour
{
    [SerializeField] protected int moveStrength = 30;
    public static bool moveAllowed = true;
    protected bool isOnGround = true;
    protected Vector2 halfPlayerSize = default;
    public static bool reachedEnd { get; protected set; }
    public static bool isOnScreen { get; protected set; }
    public static Vector3 currentPosition { get; protected set; }
    protected static Vector2 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    private void Update()
    {
        currentPosition = transform.position;
    }

    protected virtual IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded
        while (GameNS::StaticData.gameUI.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameNS::StaticData.gameUI.scoreCountText.GetComponent<Score>().OnGameLevelOpen();

        while (!reachedEnd)
        {
            if (moveAllowed)
            {
                transform.position += new Vector3(0.05f * moveStrength, 0f) * Time.deltaTime;
                Camera.main.transform.position += new Vector3(0.05f * moveStrength, 0f) * Time.deltaTime;
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

    public static bool respawnedAtCheckpoint { get; protected set; }

    protected void OnGameOver()
    {
        if (Quiz.checkpoint == null || SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameNS::StaticData.gameUI.OnViewChanged(false, true);
        }
        else if(Quiz.checkpoint != null)
        {
            //GameNS::StaticData.gameUI.quizTransform.gameObject.SetActive(false);
            //moveAllowed = true;
            //quizCollider.quizActive = false;

            transform.position = new Vector3(Quiz.checkpoint.position.x, Quiz.checkpoint.position.y, transform.position.z);

            if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Quiz.checkpoint.position.y + cameraOffset.y, Camera.main.transform.position.z);
                Score.value = System.Convert.ToInt32(Quiz.checkpoint.score);
                GameNS::StaticData.gameUI.scoreCountText.text = Score.value.ToString();
            }
            else
            {
                Camera.main.transform.position = new Vector3(Quiz.checkpoint.position.x + cameraOffset.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GameNS::StaticData.gameUI.leftTopSlider.value = PlayerLvl01Human.s_wingHealth;
                    foreach (GameObject g in GameObject.FindGameObjectsWithTag("Wax"))
                    {
                        g.GetComponent<BoxCollider2D>().enabled = true;
                        g.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                else if (SceneManager.GetActiveScene().buildIndex == 3)
                {
                    PlayerLvl03Muveszetek.index = System.Convert.ToInt32(Quiz.checkpoint.other[0]);
                    PlayerLvl03Muveszetek.heldAltAtStart = false;
                }
            }

            respawnedAtCheckpoint = true;
        }
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
    }
}
