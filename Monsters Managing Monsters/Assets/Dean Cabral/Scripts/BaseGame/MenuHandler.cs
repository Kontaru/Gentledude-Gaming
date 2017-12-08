using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider loadingSlider;
    public GameObject loadScreen;
    public GameObject helpScreen;
    public GameObject settingsScreen;

    private bool BL_helpVisible = false;
    private bool BL_loadVisible = false;
    private bool BL_settingsVisible = false;

    public void NewGame()
    {
        StartCoroutine(LoadAsync(1));
    }

    public void LoadGame()
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

        if (BL_loadVisible) StartCoroutine(HideScreen(loadScreen));
        else StartCoroutine(ShowScreen(loadScreen));

        BL_loadVisible = !BL_loadVisible;
    }

    public void LoadSettings()
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

        if (BL_settingsVisible) StartCoroutine(HideScreen(settingsScreen));
        else StartCoroutine(ShowScreen(settingsScreen));

        BL_settingsVisible = !BL_settingsVisible;
    }

    public void LoadInstructions()
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

        if (BL_helpVisible) StartCoroutine(HideScreen(helpScreen));
        else StartCoroutine(ShowScreen(helpScreen));

        BL_helpVisible = !BL_helpVisible;
    }
    

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator LoadAsync(int _sceneIndex)
    {
        float progress = 0;
        loadingScreen.SetActive(true);

        while (progress < 1)
        {
            progress += Time.deltaTime / 3f;
            loadingSlider.value = progress;
            yield return null;
        }

        if (progress >= 1) SceneManager.LoadSceneAsync(_sceneIndex);
    }

    IEnumerator ShowScreen(GameObject screen)
    {
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
    }
}
