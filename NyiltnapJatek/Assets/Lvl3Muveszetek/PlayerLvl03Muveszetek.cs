using System.Collections;
using UnityEngine;
using GameNS = GameNS;

public class PlayerLvl03Muveszetek : Player
{
    [SerializeField] private int index = 0; 
    [SerializeField] private float kottaGap = 8f;
    [SerializeField] private float waitSecond = 12f;
    private float i = 0f;

    private bool altUp = false;

    // Start is called before the first frame update
    void Start()
    {
        GameNS::StaticData.gameUI.scoreCountText.text = "0";

        levelCompletionPanelParent = GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.GetComponent<LevelCompletionUI>();
        if (levelCompletionPanelParent != null) levelCompletionPanelParent.CallPanel(false);

        waitSecond = 1f / waitSecond;
        StartCoroutine(Move()); // Inherited, automatic Move
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            altUp = true;
            StartCoroutine(move((float)index - i));
        }
        if (!altUp)
        {
            if (Input.GetKey(KeyCode.W) && index != 5)
            {
                StartCoroutine(move(kottaGap));
            }
            else if (Input.GetKey(KeyCode.S) && index != 0)
            {
                StartCoroutine(move(-kottaGap));
            }
        }

    }

    IEnumerator move(float num)
    { 
        if (!altUp) num /= 2;
        for (i = 0f; i < num && !altUp; i += 0.5f)
        {
            transform.position += new Vector3(num, 0f);
            yield return new WaitForSeconds(waitSecond);
        }
        altUp = false;
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;

        //if (Camera.main != null)
        //{
        //    if (transform.position.y < Camera.main.transform.position.y - Camera.main.orthographicSize)
        //    {
        //        OnGameOver();
        //    }
        //}
    }
}
