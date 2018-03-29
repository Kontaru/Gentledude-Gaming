using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider loadingSlider;
    public GameObject loadScreen;
    public GameObject minigameScreen;
    public GameObject helpScreen;
    public GameObject settingsScreen;
    public GameObject[] saveSlots;
    public Text loadingText;
    public GameObject[] splashArtArray;
    public Image fadeFX;

    private AsyncOperation ao;
    private bool BL_helpVisible = false;
    private bool BL_loadVisible = false;
    private bool BL_minigamesVisible = false;
    private bool BL_settingsVisible = false;

    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (SaveLoadHandler.instance.LoadData())
        {
            int day = GameManager.instance.day;
            int score = GameManager.instance.score;

            saveSlots[0].transform.Find("SaveTitle").GetComponent<Text>().text = "[1] Save Game";
            saveSlots[0].transform.Find("SaveInfo").GetComponent<Text>().text = "Day " + day + ": Score " + score;
        }
        else
        {
            foreach (GameObject g in saveSlots)
            {
                g.transform.Find("SaveTitle").GetComponent<Text>().text = "[1] Empty Save Slot";
                g.transform.Find("SaveInfo").GetComponent<Text>().text = "Day 0: Score 0000";
            }
        }
    }

    public void ClearSaves()
    {
        if (SaveLoadHandler.instance.DeleteData()) LoadData();
    }

    public void NewGame()
    {
        ClearSaves();
        StartCoroutine(LoadAsync(1));
    }

    public void LoadGame()
    {
        if (saveSlots[0].transform.Find("SaveTitle").GetComponent<Text>().text == "[1] Save Game") StartCoroutine(LoadAsync(2));
    }

    public void LoadGamePanel()
    {
        if (BL_helpVisible)
        {
            StartCoroutine(HideScreen(helpScreen));
            BL_helpVisible = !BL_helpVisible;
        }
        else if (BL_settingsVisible)
        {
            StartCoroutine(HideScreen(settingsScreen));
            BL_settingsVisible = !BL_settingsVisible;
        }
        else if (BL_minigamesVisible)
        {
            StartCoroutine(HideScreen(minigameScreen));
            BL_minigamesVisible = !BL_minigamesVisible;
        }

        if (BL_loadVisible) StartCoroutine(HideScreen(loadScreen));
        else StartCoroutine(ShowScreen(loadScreen));

        BL_loadVisible = !BL_loadVisible;
    }

    public void LoadMinigamesPanel()
    {
        if (BL_helpVisible)
        {
            StartCoroutine(HideScreen(helpScreen));
            BL_helpVisible = !BL_helpVisible;
        }
        else if (BL_settingsVisible)
        {
            StartCoroutine(HideScreen(settingsScreen));
            BL_settingsVisible = !BL_settingsVisible;
        }
        else if (BL_loadVisible)
        {
            StartCoroutine(HideScreen(loadScreen));
            BL_loadVisible = !BL_loadVisible;
        }

        if (BL_minigamesVisible) StartCoroutine(HideScreen(minigameScreen));
        else StartCoroutine(ShowScreen(minigameScreen));

        BL_minigamesVisible = !BL_minigamesVisible;

    }
    public void LoadSettingsPanel()
    {
        if (BL_helpVisible)
        {
            StartCoroutine(HideScreen(helpScreen));
            BL_helpVisible = !BL_helpVisible;
        }
        else if (BL_loadVisible)
        {
            StartCoroutine(HideScreen(loadScreen));
            BL_loadVisible = !BL_loadVisible;
        }
        else if (BL_minigamesVisible)
        {
            StartCoroutine(HideScreen(minigameScreen));
            BL_minigamesVisible = !BL_minigamesVisible;
        }

        if (BL_settingsVisible) StartCoroutine(HideScreen(settingsScreen));
        else StartCoroutine(ShowScreen(settingsScreen));

        BL_settingsVisible = !BL_settingsVisible;
    }

    public void LoadInstructionsPanel()
    {
        if (BL_settingsVisible)
        {
            StartCoroutine(HideScreen(settingsScreen));
            BL_settingsVisible = !BL_settingsVisible;
        }
        else if (BL_loadVisible)
        {
            StartCoroutine(HideScreen(loadScreen));
            BL_loadVisible = !BL_loadVisible;
        }
        else if (BL_minigamesVisible)
        {
            StartCoroutine(HideScreen(minigameScreen));
            BL_minigamesVisible = !BL_minigamesVisible;
        }

        if (BL_helpVisible) StartCoroutine(HideScreen(helpScreen));
        else StartCoroutine(ShowScreen(helpScreen));

        BL_helpVisible = !BL_helpVisible;
    }

    public void StartMiniGameOne()
    {
        GameManager.instance.LoadScene(4);
    }

    public void StartMiniGameTwo()
    {
        GameManager.instance.LoadScene(5);
    }

    public void StartMiniGameThree()
    {
        GameManager.instance.LoadScene(6);
    }

    public void StartMiniGameOneH()
    {
        GameManager.instance.LoadScene(7);

    }
    public void Exit()
    {
        Application.Quit();
    }

    private void HideAllSplashArt()
    {
        for (int i = 0; i < splashArtArray.Length; i++)
        {
            splashArtArray[i].SetActive(false);
        }
    }

    IEnumerator LoadAsync(int _sceneIndex)
    {
        loadingScreen.SetActive(true);

        ao = SceneManager.LoadSceneAsync(_sceneIndex);
        ao.allowSceneActivation = false;
        StartCoroutine(CycleSplashArt());

        while (!ao.isDone)
        {
            loadingSlider.value = ao.progress;

            if (ao.progress >= 0.9f)
            {
                loadingSlider.value = 1;
                loadingText.text = "Scene Loaded. Press 'Space' to continue.";
                if (Input.GetKeyDown(KeyCode.Space)) ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    IEnumerator CycleSplashArt()
    {
        for (int i = 0; i < splashArtArray.Length; i++)
        {
            HideAllSplashArt();

            splashArtArray[i].SetActive(true);
            StartCoroutine(FadeImageToZeroAlpha(2, fadeFX));
            yield return new WaitForSeconds(2);            
            StartCoroutine(FadeImageToFullAlpha(3, fadeFX));
            yield return new WaitForSeconds(3);            
        }

        StartCoroutine(CycleSplashArt());
    }

    IEnumerator ShowScreen(GameObject screen)
    {
        screen.SetActive(true);

        float lerpTime = 0;
        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * 2;
            screen.GetComponent<RectTransform>().localPosition = Vector3.Lerp(new Vector3(0, -335, 0), new Vector3(0, 0, 0), lerpTime);

            yield return null;
        }
    }

    IEnumerator HideScreen(GameObject screen)
    {
        float lerpTime = 0;
        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * 2;
            screen.GetComponent<RectTransform>().localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -335, 0), lerpTime);

            yield return null;
        }

        screen.SetActive(false);
    }

    IEnumerator FadeImageToFullAlpha(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    IEnumerator FadeImageToZeroAlpha(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
