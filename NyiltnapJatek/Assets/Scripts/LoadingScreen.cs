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
    public static bool finishedLoading { get; set; }
    public static bool startedLoading { get; private set; }

    private void Start()
    {
        startedLoading = false;
        GameUI.ToggleChildren(this.gameObject, false);
        GameNS::StaticData.loadingScreen = this;
    }

    public void LoadLevel(int sceneIndex)
    {
        startedLoading = true;
        finishedLoading = false;
        hintText.text = "";
        GameUI.ToggleChildren(this.gameObject, true);

        GameNS::StaticData.gameUI.mainMenuTransform.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.levelSelectionTransform.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.levelCompletionPanelText.transform.parent.gameObject.SetActive(false);

        SceneManager.LoadScene(sceneIndex);
        GameNS::StaticData.gameUI.scoreCountText.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.leftTopSlider.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.quizTransform.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.bulletCountText.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.correctAnswerText.gameObject.SetActive(false);
        StartCoroutine(Load(sceneIndex == 0)); // Is Scene Index the Main Menu Index?
    }

    int loadTickAmount = 250;

    IEnumerator Load(bool isLoadingMainMenu)
    {
        GameNS::StaticData.gameUI.keyGuide.gameObject.SetActive(false);
        GameNS::StaticData.gameUI.scoreCountText.gameObject.SetActive(false);

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
        GameNS::StaticData.gameUI.OnViewChanged(isLoadingMainMenu, false);
    }
}
