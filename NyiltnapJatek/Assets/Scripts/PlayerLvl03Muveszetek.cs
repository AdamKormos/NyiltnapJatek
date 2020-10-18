using System;
using System.Collections;
using UnityEngine;
using GameNS = GameNS;

public class PlayerLvl03Muveszetek : Player
{
    [SerializeField] private float jumpStr = 0.5f;
    [SerializeField] private int curRow = 0;

    // Start is called before the first frame update
    void Start()
    {
        levelCompletionPanelParent = GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.GetComponent<LevelCompletionUI>();
        if (levelCompletionPanelParent != null) levelCompletionPanelParent.CallPanel(false);
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override IEnumerator Move()
    {
        while (!LoadingScreen.finishedLoading && LoadingScreen.startedLoading) { yield return new WaitForSeconds(0.1f); } // Freeze movement until the scene isn't loaded

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
