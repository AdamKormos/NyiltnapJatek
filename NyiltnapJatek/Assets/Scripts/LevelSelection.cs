using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameNS = GameNS;

[System.Serializable]
public struct LevelPanel
{
    [SerializeField] public string name;
    [SerializeField] public Sprite img;
    [SerializeField] public Menu.Scenes sceneToLoad;
}

public class LevelSelection : MonoBehaviour
{
    [SerializeField] float xOffsetBetweenPanels = 60f;
    [SerializeField] GameObject samplePanelBackground = default;
    [SerializeField] Text samplePanelText = default;
    [SerializeField] Image samplePanelImage = default;
    [SerializeField] List<LevelPanel> levelPanels = new List<LevelPanel>();
    Vector2 panelStartPosition = default;
    int currentIndex = 0;

    private void Start()
    {
        samplePanelBackground.SetActive(false);
        samplePanelImage.gameObject.SetActive(false);
        samplePanelText.gameObject.SetActive(false);

        panelStartPosition = samplePanelBackground.transform.position;

        for(int i = 0; i < levelPanels.Count; i++)
        {
            GameObject panelBackground = Instantiate(samplePanelBackground, 
                                                new Vector2(panelStartPosition.x + i * xOffsetBetweenPanels, samplePanelBackground.transform.position.y), Quaternion.identity, transform);
            GameObject panelImage = Instantiate(samplePanelImage.gameObject, 
                                                new Vector2(panelStartPosition.x + i * xOffsetBetweenPanels, samplePanelImage.transform.position.y), Quaternion.identity, transform);
            panelImage.GetComponent<Image>().sprite = levelPanels[i].img;

            GameObject panelText = Instantiate(samplePanelText.gameObject, 
                                                new Vector2(panelStartPosition.x + i * xOffsetBetweenPanels, samplePanelText.transform.position.y), Quaternion.identity, transform);
            panelText.GetComponent<Text>().text = levelPanels[i].name;

            panelBackground.SetActive(true);
            panelImage.SetActive(true);
            panelText.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex++;
            }
            else
            {
                transform.position += new Vector3(xOffsetBetweenPanels, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex++;
            if (currentIndex > levelPanels.Count-1)
            {
                currentIndex--;
            }
            else
            {
                transform.position -= new Vector3(xOffsetBetweenPanels, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            GameNS::StaticData.loadingScreen.LoadLevel(levelPanels[currentIndex].sceneToLoad);
        }
    }
}
