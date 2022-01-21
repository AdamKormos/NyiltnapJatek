using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// TODO: Refactor if (respawnedAtCheckpoint)s in OnBecameVisible()s. They are most likely unnecessary for most player classes.
/// <summary>
/// The base class for all Player kinds in the game.
/// </summary>
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

    /// <summary>
    /// Base movement code with freezed movement. Freezing lasts as long as gameplay doesn't start (level hint bar isn't disabled). Once gameplay starts the score script
    /// initiates the score counter and movement is available as long as the player hasn't reached the end of the level. At that point, the player will move along as long
    /// as they're visible.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Move()
    {
        // TODO: Try this with a static bool refering to the LoadingScreen gameobject's activity? 
        // We would be at the same memory usage as get;private set; is 2 bools, but might reduce complexity tho
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) // Loading screen started loading, but hasn't finished it yet. 
        { // Freeze movement until the scene isn't loaded 
            yield return new WaitForSeconds(0.1f); 
        }
        while (GameUI.instance.levelHintBar.gameObject.activeSelf) { yield return new WaitForSeconds(0.1f); }
        GameUI.instance.scoreCountText.GetComponent<Score>().OnGameLevelOpen();

        while (!reachedEnd)
        {
            if (moveAllowed)
            {
                transform.position += new Vector3(0.05f * moveStrength, 0f) * Time.deltaTime;
                Camera.main.transform.position += new Vector3(0.05f * moveStrength, 0f) * Time.deltaTime;
                yield return new WaitForSeconds(1 / 60);
            }
            else yield return new WaitForSeconds(0.1f);
        }

        while (isOnScreen)
        {
            transform.position += new Vector3(0.05f * moveStrength, 0f) * Time.deltaTime;
            yield return new WaitForSeconds(1 / 60);
        }
    }

    public static bool respawnedAtCheckpoint { get; set; }

    /// <summary>
    /// Called on game over. The player either spawns at a checkpoint or begins the level from scratch.
    /// </summary>
    protected void OnGameOver()
    {
        if (Quiz.checkpoint == null || SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameUI.instance.OnViewChanged(false, true);
        }
        else if(Quiz.checkpoint != null)
        {
            //GameUI.instance.quizTransform.gameObject.SetActive(false);
            //moveAllowed = true;
            //quizCollider.quizActive = false;

            transform.position = new Vector3(Quiz.checkpoint.position.x, Quiz.checkpoint.position.y, transform.position.z);

            if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Quiz.checkpoint.position.y + cameraOffset.y, Camera.main.transform.position.z);
                Score.value = System.Convert.ToInt32(Quiz.checkpoint.score);
                GameUI.instance.scoreCountText.text = Score.value.ToString();
            }
            else
            {
                Camera.main.transform.position = new Vector3(Quiz.checkpoint.position.x + cameraOffset.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    GameUI.instance.leftTopSlider.value = PlayerLvl01Human.s_wingHealth;
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
                    PlayerLvl03Muveszetek.Moving = false;
                }
            }

            respawnedAtCheckpoint = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelEnding")) { reachedEnd = true; }
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
