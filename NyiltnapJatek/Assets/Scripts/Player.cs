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
    public static bool reachedEnd { get; private set; }
    public static bool isOnScreen { get; private set; }

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
        if (LevelSelection.maxIndex < LevelSelection.currentIndex) LevelSelection.maxIndex = LevelSelection.currentIndex;

        if (levelCompletionPanelParent != null)
        {
            levelCompletionPanelParent.CallPanel(true);
        }
    }
}
