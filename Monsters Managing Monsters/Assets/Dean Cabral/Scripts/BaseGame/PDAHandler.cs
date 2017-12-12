using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDAHandler : MonoBehaviour {

    public GameObject homeScreen;
    public GameObject mapScreen;
    public GameObject statsScreen;
    public GameObject tasksScreen;

    public bool BL_PDAactive;

    private void Start()
    {
        GetComponent<RectTransform>().localPosition = new Vector3(249, -380, 0);
    }

    // Update is called once per frame
    void Update () {

        CheckInput();
        TogglePDA();

	}

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.M)) ShowMap();
        if (Input.GetKeyDown(KeyCode.N)) ShowTasks();
        if (Input.GetKeyDown(KeyCode.B)) ShowStats();
    }
    
    private void TogglePDA()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (BL_PDAactive) StartCoroutine(HidePDA());
            else StartCoroutine(ShowPDA());

            BL_PDAactive = !BL_PDAactive;
        }
    }
    public void OnClickHome()
    {
        ShowHome();
    }

    public void OnClickMap()
    {
        ShowMap();
    }

    public void OnClickStats()
    {
        ShowStats();
    }

    public void OnClickTasks()
    {
        ShowTasks();
    }

    public void OnClickClose()
    {
        StartCoroutine(HidePDA());
        BL_PDAactive = false;
    }

    private void HideAllScreens()
    {
        mapScreen.SetActive(false);
        statsScreen.SetActive(false);
        tasksScreen.SetActive(false);
    }

    private void ShowHome()
    {
        HideAllScreens();
        homeScreen.SetActive(true);
    }

    private void ShowMap()
    {
        HideAllScreens();
        mapScreen.SetActive(true);
    }

    private void ShowStats()
    {
        HideAllScreens();
        statsScreen.SetActive(true);
    }

    private void ShowTasks()
    {
        HideAllScreens();
        tasksScreen.SetActive(true);
    }

    IEnumerator ShowPDA()
    {
        float lerpTime = 0;
        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * 3;
            GetComponent<RectTransform>().localPosition = Vector3.Lerp(new Vector3(249, -380, 0), new Vector3(249, 50, 0), lerpTime);

            yield return null;
        }
    }

    IEnumerator HidePDA()
    {
        HideAllScreens();

        float lerpTime = 0;
        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * 3;
            GetComponent<RectTransform>().localPosition = Vector3.Lerp(new Vector3(249, 50, 0), new Vector3(249, -380, 0), lerpTime);

            yield return null;
        }
    }
}
