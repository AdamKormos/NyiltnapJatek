using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameNS = GameNS;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] float minimumLoadTime = 4f, maximumLoadTime = 10f;
    [SerializeField] Text hintText = default;
    [SerializeField] string[] hints = default;
    public static bool finishedLoading { get; private set; }
    public static bool startedLoading { get; private set; }

    private void Start()
    {
        startedLoading = false;
        GameUI.ToggleChildren(this.gameObject, false);
        GameNS::StaticData.loadingScreen = this;
    }

    public void LoadLevel(Menu.Scenes sceneEnum)
    {
        startedLoading = true;
        finishedLoading = false;
        hintText.text = "";
        GameUI.ToggleChildren(this.gameObject, true);
        GameNS::StaticData.gameUI.mainMenuTransform.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.levelSelectionTransform.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.gameObject.SetActive(false);
        SceneManager.LoadScene((int)sceneEnum);
        GameNS::StaticData.gameUI.timerText.gameObject.SetActive(false);
        StartCoroutine(Load(sceneEnum == Menu.Scenes.mainMenu));
    }

    int loadTickAmount = 250;

    IEnumerator Load(bool isLoadingMainMeenu)
    {
        hintText.text = hints[Random.Range(0, hints.Length)];
        float val = Random.Range(minimumLoadTime, maximumLoadTime);
        Debug.Log(val);

        GameNS::StaticData.gameUI.loadingScreenSlider.value = 0;
        GameNS::StaticData.gameUI.loadingScreenSlider.maxValue = loadTickAmount;

        for (int i = 0; i < loadTickAmount; i++)
        {
            GameNS::StaticData.gameUI.loadingScreenSlider.value += 1;
            yield return new WaitForSeconds(val / loadTickAmount);
        }

        finishedLoading = true;
        startedLoading = false;
        GameUI.ToggleChildren(this.gameObject, false);
        GameNS::StaticData.gameUI.OnViewChanged(isLoadingMainMeenu);
    }
}
