using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDAHandler : MonoBehaviour {

    public GameObject homeScreen;
    public GameObject mapScreen;
    public GameObject statsScreen;
    public GameObject tasksScreen;
    public GameObject gamesScreen;
    public GameObject loadingScreen;
    public GameObject renderCam;
    public GameObject[] minigames;
    private Animator animator;

    public int minigameIndex;
    public bool BL_PDAactive;
    public bool BL_PDAlandscape;

    private void Start()
    {
        GetComponent<RectTransform>().localPosition = new Vector3(249, -380, 0);
        animator = GetComponentInChildren<Animator>();
        minigameIndex = 0;
    }

    // Update is called once per frame
    void Update () {

        CheckInput();
	}

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.P)) TogglePDA();
        if (Input.GetKeyDown(KeyCode.M)) ShowMap();
        if (Input.GetKeyDown(KeyCode.N)) ShowTasks();
        if (Input.GetKeyDown(KeyCode.B)) ShowStats();        
    }
    
    private void TogglePDA()
    {
        if (BL_PDAlandscape) return;

        if (BL_PDAactive) animator.SetBool("BL_ShowPDA", false);
        else animator.SetBool("BL_ShowPDA", true);

        ShowHome();
        BL_PDAactive = !BL_PDAactive;
    }

    private void ToggleLandscape()
    {
        if (!BL_PDAactive) return;

        if (BL_PDAlandscape)
        {            
            GameManager.instance.PixelMode = false;
            animator.SetBool("BL_Landscape", false);
            renderCam.SetActive(false);
            minigames[minigameIndex].SetActive(false);
        }
        else
        {
            GameManager.instance.PixelMode = true;
            animator.SetBool("BL_Landscape", true);
            StartCoroutine(WaitAndDisplay(1.5f));            
        }
        
        BL_PDAlandscape = !BL_PDAlandscape;
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

    public void OnClickGames()
    {
        ShowMinigames();
    }

    public void OnClickSave()
    {
        SaveLoadHandler.instance.SaveData();
    }

    public void OnClickLoad()
    {
        SaveLoadHandler.instance.LoadData();
    }

    public void OnClickClose()
    {
        animator.SetBool("BL_ShowPDA", false);
        BL_PDAactive = false;
    }

    public void StartDD()
    {
        minigameIndex = 0;
        ToggleLandscape();
    }

    public void StartPS()
    {
        minigameIndex = 1;
        ToggleLandscape();
    }

    private void HideAllScreens()
    {
        mapScreen.SetActive(false);
        statsScreen.SetActive(false);
        tasksScreen.SetActive(false);
        gamesScreen.SetActive(false);
        loadingScreen.SetActive(false);
    }

    private void ShowHome()
    {
        HideAllScreens();
        homeScreen.SetActive(true);

        if (BL_PDAlandscape) ToggleLandscape();
    }

    private void ShowMap()
    {
        HideAllScreens();
        mapScreen.SetActive(true);
        if (!BL_PDAactive) animator.SetBool("BL_ShowPDA", true);

        BL_PDAactive = true;
    }

    private void ShowStats()
    {
        HideAllScreens();
        statsScreen.SetActive(true);
        if (!BL_PDAactive) animator.SetBool("BL_ShowPDA", true);

        BL_PDAactive = true;
    }

    private void ShowTasks()
    {
        HideAllScreens();
        tasksScreen.SetActive(true);
        if (!BL_PDAactive) animator.SetBool("BL_ShowPDA", true);

        BL_PDAactive = true;
    }

    private void ShowMinigames()
    {
        HideAllScreens();
        gamesScreen.SetActive(true);
    }

    private void ShowLoading()
    {
        HideAllScreens();
        loadingScreen.SetActive(true);
    }

    IEnumerator WaitAndDisplay(float seconds)
    {
        ShowLoading();
        yield return new WaitForSeconds(seconds);
        renderCam.SetActive(true);
        minigames[minigameIndex].SetActive(true);
    }
}
