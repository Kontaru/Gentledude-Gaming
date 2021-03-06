﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDAHandler : MonoBehaviour {

    public bool BL_isTutorial;
    public bool BL_hasPDA;

    public GameObject oldPDA;
    public GameObject homeScreen;
    public GameObject mapScreen;
    public GameObject statsScreen;
    public GameObject tasksScreen;
    public GameObject taskBriefScreen;
    public GameObject gamesScreen;
    public GameObject instructionScreen;
    public GameObject loadingScreen;
    public GameObject pauseScreen;
    public GameObject renderCam;

    public GameObject[] minigames;
    public GameObject[] activeTasks;

    public GameObject questContainer;
    public GameObject questSlot;

    public Button homeBtn, closeBtn;
    public Slider volSlider;
    public Text tasksText, volText, tbTitleText, tbText, tbInfoText;
    public Text PDAminute, PDAhour;
    public Text HRstat, ITstat, JNstat, MKstat, FNstat, OVRstat, SECstat;

    private Animator animator;

    public GameObject noQuestsLabel;
    private List<GameObject> completedTasks;
    private string tasks;
    private int taskIndex;

    public int minigameIndex;
    public bool BL_PDAactive;
    public bool BL_PDAlandscape;
    public bool BL_Pause;    

    public static PDAHandler instance;

    #region Typical Singleton Format

    void Awake()
    {

        //Singleton stuff
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        completedTasks = new List<GameObject>();
        minigameIndex = 0;
    }

    void Update () {

        CheckInput();
        UpdateUI();

    }

    private void CheckInput()
    {
        if (!BL_hasPDA) return;

        if (Input.GetKeyDown(KeyCode.P)) TogglePDA();
        if (Input.GetKeyDown(KeyCode.M)) ShowMap();
        if (Input.GetKeyDown(KeyCode.N)) ShowTasks();
        if (Input.GetKeyDown(KeyCode.B)) ShowStats();
    }

    private void UpdateUI()
    {
        if (volText != null) volText.text = "Volume " + volSlider.value + "%";        
    }

    private void RefreshTasksList()
    {
        Task[] tasksArr = CurrentTasks.instance.currentTask;
        tasks = "";

        for (int i = 0; i < tasksArr.Length; i++)
        {
            if (i > 0) tasks += "\n" + "[" + tasksArr[i].Quest_ID + "]" + " " + tasksArr[i].name;
            else tasks += "[" + tasksArr[i].Quest_ID + "]" + " " + tasksArr[i].name;
        }

        tasksText.text = tasks;
    }

    private void UpdateActiveTasks()
    {
        int length = BL_isTutorial ? 1 : activeTasks.Length;

        for (int i = 0; i < length; i++)
        {
            Text title = activeTasks[i].transform.GetChild(0).GetComponent<Text>();
            Text id = activeTasks[i].transform.GetChild(1).GetComponent<Text>();
            Task task = CurrentTasks.instance.currentTask[i];

            string taskName = task.name;
            int taskID = task.Quest_ID;

            if (taskID != -1)
            {
                title.text = taskName;
                id.text = taskID.ToString();
            }
            else
            {
                title.text = "No Quest Found";
                id.text = "";
            }
        }
    }

    public void TogglePDA()
    {
        if (BL_PDAlandscape) return;

        if (BL_PDAactive) animator.SetBool("BL_ShowPDA", false);
        else animator.SetBool("BL_ShowPDA", true);

        ShowHome();
        if (oldPDA != null) oldPDA.SetActive(false);
        BL_PDAactive = !BL_PDAactive;
    }

    private void ToggleOldPDA()
    {
        if (BL_PDAactive) oldPDA.GetComponent<Animator>().SetBool("BL_ShowPDA", false);
        else oldPDA.GetComponent<Animator>().SetBool("BL_ShowPDA", true);

        BL_PDAactive = !BL_PDAactive;
    }   

    private void ToggleMinigames(bool quickStart)
    {
        if (!BL_PDAactive) return;

        if (BL_PDAlandscape)
        {
            GameManager.instance.PixelMode = false;
            animator.SetBool("BL_Landscape", false);
            renderCam.SetActive(false);
            instructionScreen.SetActive(false);

            homeBtn.enabled = true;
            closeBtn.enabled = true;

            minigames[minigameIndex].SetActive(false);
        }
        else
        {
            GameManager.instance.PixelMode = true;
            animator.SetBool("BL_Landscape", true);

            homeBtn.enabled = false;
            closeBtn.enabled = false;

            if (!quickStart) StartCoroutine(WaitAndDisplay(1.5f, false));
            else StartCoroutine(WaitAndDisplay(3f, false));
        }        
    }

    public void PauseGame()
    {
        BL_Pause = !BL_Pause;
        GameManager.instance.PauseGame();
        RefreshTasksList();

        if (BL_Pause)
        {
            animator.speed = 3;

            if (!BL_PDAactive) animator.SetBool("BL_ShowPDA", true);
            animator.SetBool("BL_Landscape", true);
            StartCoroutine(WaitAndDisplay(1f, true));

            BL_PDAactive = true;
            BL_PDAlandscape = true;            
        }
        else
        {
            animator.speed = 1;

            animator.SetBool("BL_Landscape", false);
            animator.SetBool("BL_ShowPDA", false);

            pauseScreen.SetActive(false);
            BL_PDAactive = false;
            BL_PDAlandscape = false;

            ShowHome();
        }          
    }

    public void OnClickHome()
    {
        ShowHome();
        AudioManager.instance.Play("PDA Button Click");
    }

    public void OnClickMap()
    {
        ShowMap();
        AudioManager.instance.Play("PDA Button Click");
    }

    public void OnClickStats()
    {
        ShowStats();
        AudioManager.instance.Play("PDA Button Click");
    }

    public void OnClickTasks()
    {
        ShowTasks();
        AudioManager.instance.Play("PDA Button Click");
    }

    public void OnClickTask(int index)
    {
        taskIndex = index;
        ShowTaskBrief(taskIndex);
        AudioManager.instance.Play("PDA Button Click");
    }

    public void OnClickGames()
    {
        ShowMinigames();
        AudioManager.instance.Play("PDA Button Click");
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
        if (BL_PDAlandscape && !BL_Pause) return;

        ShowHome();
        animator.SetBool("BL_ShowPDA", false);
        BL_PDAactive = false;
    }

    public void LocateUser()
    {
        StartCoroutine(LocateEntity(3));
        AudioManager.instance.Play("PDA Locate");
    }

    public void LocateObjective()
    {
        StartCoroutine(LocateStep(3));
        AudioManager.instance.Play("PDA Locate");
    }

    public void EnableMinigameRender()
    {
        renderCam.SetActive(true);
        minigames[minigameIndex].SetActive(true);
    }

    public void StartDD()
    {
        StartMinigame(0, false);
        AudioManager.instance.Play("PDA Button Click");
    }

    public void StartPP()
    {
        StartMinigame(1, false);
        AudioManager.instance.Play("PDA Button Click");
    }

    public void StartCI()
    {
        StartMinigame(2, false);
        AudioManager.instance.Play("PDA Button Click");
    }

    public void StartPC()
    {
        StartMinigame(3, false);
        AudioManager.instance.Play("PDA Button Click");
    }

    public void StartSS()
    {
        StartMinigame(4, false);
        AudioManager.instance.Play("PDA Button Click");
    }

    public void StartOE()
    {
        StartMinigame(5, false);
        AudioManager.instance.Play("PDA Button Click");
    }

    public void StartMinigame(int index, bool quickStart)
    {
        minigameIndex = index;

        if (!BL_PDAactive) animator.SetBool("BL_ShowPDA", true);
        BL_PDAactive = true;

        ToggleMinigames(quickStart);

        BL_PDAlandscape = !BL_PDAlandscape;
    }

    public void MinigameComplete(bool win)
    {
        BL_Pause = false;
        ToggleMinigames(false);        
        ShowHome();
        OnClickClose();
    }

    private void HideAllScreens()
    {
        mapScreen.SetActive(false);
        statsScreen.SetActive(false);
        tasksScreen.SetActive(false);
        taskBriefScreen.SetActive(false);
        gamesScreen.SetActive(false);
        loadingScreen.SetActive(false);
    }

    private void ShowHome()
    {     
        HideAllScreens();
        PDAhour.text = DayCycle.instance.Hour.text;
        PDAminute.text = DayCycle.instance.Minute.text;
        homeScreen.SetActive(true);

        if (BL_PDAlandscape) animator.SetBool("BL_Landscape", false);
        if (BL_Pause) PauseGame();

        BL_PDAlandscape = false;
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
        HRstat.text = "+" + GameManager.instance.PL_HR.ToString();
        ITstat.text = "+" + GameManager.instance.PL_IT.ToString();
        JNstat.text = "+" + GameManager.instance.PL_Janitorial.ToString();
        MKstat.text = "+" + GameManager.instance.PL_Marketing.ToString();
        FNstat.text = "+" + GameManager.instance.PL_Finance.ToString();
        OVRstat.text = "+" + GameManager.instance.PL_Overseas.ToString();
        SECstat.text = "+" + GameManager.instance.PL_Security.ToString();
        statsScreen.SetActive(true);

        if (!BL_PDAactive) animator.SetBool("BL_ShowPDA", true);

        BL_PDAactive = true;
    }

    private void ShowTasks()
    {
        HideAllScreens();
        UpdateActiveTasks();
        tasksScreen.SetActive(true);
        if (!BL_PDAactive) animator.SetBool("BL_ShowPDA", true);

        BL_PDAactive = true;
    }

    private void ShowTaskBrief(int index)
    {
        if (CurrentTasks.instance.currentTask[index].Quest_ID == -1) return;
        HideAllScreens();
        SetBrief(taskIndex);
        taskBriefScreen.SetActive(true);
    }

    public void ShowCompletedTasks()
    {
        foreach (GameObject go in completedTasks)
        {
            Destroy(go);
        }

        completedTasks.Clear();

        foreach (Task task in TaskManager.instance.Tasks)
        {
            if (task.Quest_Finish)
            {
                GameObject GO = Instantiate(questSlot);
                GO.transform.SetParent(questContainer.transform, false);
                completedTasks.Add(GO);

                if (task.Quest_Fail)
                {
                    GO.transform.GetChild(0).GetComponent<Text>().text = task.name;
                    GO.transform.GetChild(1).GetComponent<Text>().text = "Failed";
                    GO.transform.GetChild(1).GetComponent<Text>().color = Color.red;
                }
                else
                {
                    GO.transform.GetChild(0).GetComponent<Text>().text = task.name;
                    GO.transform.GetChild(1).GetComponent<Text>().text = "Completed";
                    GO.transform.GetChild(1).GetComponent<Text>().color = Color.green;
                }
            }            
        }

        if (completedTasks.Count > 0) noQuestsLabel.SetActive(false);
        else noQuestsLabel.SetActive(true);

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

    private void HideAllInstructions()
    {        
        Transform screen = instructionScreen.transform;
        screen.GetChild(2).gameObject.SetActive(false);
        screen.GetChild(3).gameObject.SetActive(false);
        screen.GetChild(4).gameObject.SetActive(false);
        screen.GetChild(5).gameObject.SetActive(false);
        screen.GetChild(6).gameObject.SetActive(false);
        screen.GetChild(7).gameObject.SetActive(false);
    }

    public void ShowMomText()
    {
        oldPDA.SetActive(true);
        ToggleOldPDA();
    }

    public void HideMomText()
    {
        ToggleOldPDA();
    }

    private void SetBrief(int index)
    {
        Task task = CurrentTasks.instance.currentTask[index];
        tbTitleText.text = task.name;
        tbText.text = task.ST_taskBrief;
        if (task.GO_belongsTo) tbInfoText.text = "From: " + task.GO_belongsTo.GetComponent<Attribution>().myAttribute.ToString() + "\nAction Points: " + task.IN_actionPointWeight + "\nMotivation: +" + task.IN_motivationAmount;
        else tbInfoText.text = "ERROR: No belongsTo object found";
    }

    IEnumerator WaitAndDisplay(float seconds, bool isPause)
    {
        if (!isPause) ShowLoading();

        yield return new WaitForSeconds(seconds);

        if (isPause) pauseScreen.SetActive(true);
        else
        {
            instructionScreen.SetActive(true);
            HideAllInstructions();
            instructionScreen.transform.GetChild(minigameIndex + 2).gameObject.SetActive(true);
        }        
    }

    IEnumerator LocateEntity(float seconds)
    {
        Task task = CurrentTasks.instance.currentTask[taskIndex];
        CameraFollow.instance.otherLook = task.GO_belongsTo;
        yield return new WaitForSeconds(seconds);
        CameraFollow.instance.otherLook = null;
    }

    IEnumerator LocateStep(float seconds)
    {
        Task task = CurrentTasks.instance.currentTask[taskIndex];
        if (task.Steps[task.step_tracker].Hidden)
        {
            CameraFollow.instance.ZoomedOut();
        }
        else
        {
            CameraFollow.instance.otherLook = task.Steps[task.step_tracker].requires.gameObject;
            yield return new WaitForSeconds(seconds);
            CameraFollow.instance.otherLook = null;
        }
    }
}
