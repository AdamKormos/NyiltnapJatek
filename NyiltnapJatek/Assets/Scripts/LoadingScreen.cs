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

    private void Start()
    {
        GameUI.ToggleChildren(this.gameObject, false);
        GameNS::StaticData.loadingScreen = this;
    }

    public void LoadLevel(Menu.Scenes sceneEnum)
    {
        hintText.text = "";
        GameUI.ToggleChildren(this.gameObject, true);
        SceneManager.LoadScene((int)sceneEnum);
        GameNS::StaticData.gameUI.timerText.gameObject.SetActive(false);
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        hintText.text = hints[Random.Range(0, hints.Length)];
        float val = Random.Range(minimumLoadTime, maximumLoadTime);
        Debug.Log(val);

        int tickAmount = 250;
        GameNS::StaticData.gameUI.loadingScreenSlider.maxValue = tickAmount;

        for (int i = 0; i < tickAmount; i++)
        {
            GameNS::StaticData.gameUI.loadingScreenSlider.value += 1;
            yield return new WaitForSeconds(val / tickAmount);
        }

        GameUI.ToggleChildren(this.gameObject, false);
    }
}
