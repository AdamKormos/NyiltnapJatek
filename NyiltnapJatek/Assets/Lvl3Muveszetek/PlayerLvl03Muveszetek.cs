using System.Collections;
using System.Globalization;
using UnityEngine;
using GameNS = GameNS;

public class PlayerLvl03Muveszetek : Player
{
    [SerializeField] private int index = 0; 
    [SerializeField] private float kottaGap = 8f;
    [SerializeField] private int moveTickAmount = 30;
    [SerializeField] private float waitSecond = 12f;

    private bool Moving = false;

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
        if (!Moving)
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                StartCoroutine(move(-kottaGap / 2));
            }
            else if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                StartCoroutine(move(kottaGap / 2));
            }
            else if (Input.GetKey(KeyCode.W) && index != 5)
            {
                index++;
                StartCoroutine(move(kottaGap));
            }
            else if (Input.GetKey(KeyCode.S) && index != 0)
            {
                index--;
                StartCoroutine(move(-kottaGap));
            }
        }
    }

    bool brokeAltMove = false;

    IEnumerator move(float num)
    {
        Moving = true;
        bool wasHoldingAlt = Input.GetKey(KeyCode.LeftAlt);

        for (int i = 0; i < moveTickAmount; i++)
        {
            if((wasHoldingAlt && !Input.GetKey(KeyCode.LeftAlt)) && !brokeAltMove)
            {
                brokeAltMove = true;
                StartCoroutine(move(-(num / moveTickAmount) * i));
                yield break;
            }

            transform.position += new Vector3(0, num / moveTickAmount);
            yield return new WaitForSeconds(waitSecond);
        }

        Moving = false;
        brokeAltMove = false;
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
