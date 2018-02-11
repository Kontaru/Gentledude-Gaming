using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDAHandler : MonoBehaviour {

    public GameObject homeScreen;
    public GameObject mapScreen;
    public GameObject statsScreen;
    public GameObject tasksScreen;
    public GameObject taskBriefScreen;
    public GameObject gamesScreen;
    public GameObject loadingScreen;
    public GameObject pauseScreen;
    public GameObject renderCam;
    public GameObject timeObject;
    public GameObject[] minigames;
    public GameObject[] activeTasks;

    public Button homeBtn, closeBtn;
    public Slider volSlider;
    public Text tasksText, volText, tbTitleText, tbText, tbInfoText;

    private Animator animator;

    private string tasks;
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
        GetComponent<RectTransform>().localPosition = new Vector3(249, -380, 0);
        animator = GetComponentInChildren<Animator>();
        minigameIndex = 0;
    }

    // Update is called once per frame
    void Update () {

        CheckInput();
        UpdateUI();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
        if (Input.GetKeyDown(KeyCode.P)) TogglePDA();
        if (Input.GetKeyDown(KeyCode.M)) ShowMap();
        if (Input.GetKeyDown(KeyCode.N)) ShowTasks();
        if (Input.GetKeyDown(KeyCode.B)) ShowStats();
    }

    private void UpdateUI()
    {
        volText.text = "Volume " + volSlider.value + "%";
        tasksText.text = tasks;
    }

    private void RefreshTasksList()
    {
        Task[] tasksArr = TaskManager.instance.Tasks;
        tasks = "";

        for (int i = 0; i < tasksArr.Length; i++)
        {
            if (i > 0) tasks += "\n" + "[" + tasksArr[i].Quest_ID + "]" + " " + tasksArr[i].name;
            else tasks += "[" + tasksArr[i].Quest_ID + "]" + " " + tasksArr[i].name;
        }
    }

    private void UpdateActiveTasks()
    {
        for (int i = 0; i < activeTasks.Length; i++)
        {
            Text title = activeTasks[i].transform.GetChild(0).GetComponent<Text>();
            Text id = activeTasks[i].transform.GetChild(1).GetComponent<Text>();
            Task task = CurrentTasks.currentTask[i];

            string taskName = task.name;
            int taskID = task.Quest_ID;

            title.text = taskName;
            id.text = taskID.ToString();
        }
    }
    
    private void TogglePDA()
    {
        if (BL_PDAlandscape) return;

        if (BL_PDAactive) animator.SetBool("BL_ShowPDA", false);
        else animator.SetBool("BL_ShowPDA", true);

        ShowHome();
        BL_PDAactive = !BL_PDAactive;
    }

    private void ToggleMinigames()
    {
        if (!BL_PDAactive) return;

        if (BL_PDAlandscape)
        {
            GameManager.instance.PixelMode = false;
            timeObject.SetActive(true);
            animator.SetBool("BL_Landscape", false);
            renderCam.SetActive(false);
            homeBtn.enabled = true;
            closeBtn.enabled = true;
            minigames[minigameIndex].SetActive(false);
        }
        else
        {
            GameManager.instance.PixelMode = true;
            timeObject.SetActive(false);
            animator.SetBool("BL_Landscape", true);
            homeBtn.enabled = false;
            closeBtn.enabled = false;
            StartCoroutine(WaitAndDisplay(1.5f, false));
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
            timeObject.SetActive(false);
            if (!BL_PDAactive) animator.SetBool("BL_ShowPDA", true);
            animator.SetBool("BL_Landscape", true);
            StartCoroutine(WaitAndDisplay(1f, true));
            BL_PDAactive = true;
            BL_PDAlandscape = true;            
        }
        else
        {
            animator.speed = 1;
            timeObject.SetActive(true);
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

    public void OnClickTask(int index)
    {
        ShowTaskBrief(index);
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
        if (BL_PDAlandscape && !BL_Pause) return;

        ShowHome();
        animator.SetBool("BL_ShowPDA", false);
        BL_PDAactive = false;
    }

    public void StartDD()
    {
        StartMinigame(0);
    }

    public void StartPP()
    {
        StartMinigame(1);
    }

    public void StartCI()
    {
        StartMinigame(2);
    }

    public void StartMinigame(int index)
    {
        minigameIndex = index;
        ToggleMinigames();
        BL_PDAlandscape = !BL_PDAlandscape;
    }

    public void MinigameComplete()
    {
        BL_Pause = false;
        ToggleMinigames();
        ShowHome();
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
        HideAllScreens();
        SetBrief(index);
        taskBriefScreen.SetActive(true);
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

    private void SetBrief(int index)
    {
        Task task = CurrentTasks.currentTask[index];
        tbTitleText.text = task.name;
        tbText.text = task.ST_taskBrief;
        tbInfoText.text = "From: " + /*task.GO_belongsTo.GetComponent<Attribution>().myAttribute.ToString() +*/ "\nAction Points: " + task.IN_actionPointWeight + "\nMotivation: +" + task.IN_motivationAmount;
    }

    IEnumerator WaitAndDisplay(float seconds, bool isPause)
    {
        if (!isPause) ShowLoading();
        yield return new WaitForSeconds(seconds);
        if (isPause)
        {
            pauseScreen.SetActive(true);
        }
        else
        {
            renderCam.SetActive(true);
            minigames[minigameIndex].SetActive(true);
        }
        
    }
}
