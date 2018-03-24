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
    public GameObject instructionScreen;
    public GameObject loadingScreen;
    public GameObject pauseScreen;
    public GameObject renderCam;
    public GameObject[] minigames;
    public GameObject[] activeTasks;

    public Button homeBtn, closeBtn;
    public Slider volSlider;
    public Text tasksText, volText, tbTitleText, tbText, tbInfoText;
    public Text PDAminute, PDAhour;
    public Text HRstat, ITstat, JNstat, MKstat, FNstat, OVRstat, SECstat;

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
        if (Input.GetKeyDown(KeyCode.P)) TogglePDA();
        if (Input.GetKeyDown(KeyCode.M)) ShowMap();
        if (Input.GetKeyDown(KeyCode.N)) ShowTasks();
        if (Input.GetKeyDown(KeyCode.B)) ShowStats();
    }

    private void UpdateUI()
    {
        volText.text = "Volume " + volSlider.value + "%";        
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

        tasksText.text = tasks;
    }

    private void UpdateActiveTasks()
    {
        for (int i = 0; i < activeTasks.Length; i++)
        {
            Text title = activeTasks[i].transform.GetChild(0).GetComponent<Text>();
            Text id = activeTasks[i].transform.GetChild(1).GetComponent<Text>();
            Task task = CurrentTasks.instance.currentTask[i];

            string taskName = task.name;
            int taskID = task.Quest_ID;

            if (taskID != 0)
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
    
    private void TogglePDA()
    {
        if (BL_PDAlandscape) return;

        if (BL_PDAactive) animator.SetBool("BL_ShowPDA", false);
        else animator.SetBool("BL_ShowPDA", true);

        ShowHome(false);
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
            ShowHome(false);
        }          
    }

    public void OnClickHome()
    {
        ShowHome(false);
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

        ShowHome(false);
        animator.SetBool("BL_ShowPDA", false);
        BL_PDAactive = false;
    }

    public void EnableMinigameRender()
    {
        renderCam.SetActive(true);
        minigames[minigameIndex].SetActive(true);
    }

    public void StartDD()
    {
        StartMinigame(0, false);
    }

    public void StartPP()
    {
        StartMinigame(1, false);
    }

    public void StartCI()
    {
        StartMinigame(2, false);
    }

    public void StartPC()
    {
        StartMinigame(3, false);
    }

    public void StartSS()
    {
        StartMinigame(4, false);
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
        ShowHome(false);
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

    public void ShowTutorial()
    {
        ShowTasks();
    }

    private void ShowHome(bool showText)
    {     
        HideAllScreens();
        PDAhour.text = DayCycle.instance.Hour.text;
        PDAminute.text = DayCycle.instance.Minute.text;
        homeScreen.SetActive(true);

        if (showText) homeScreen.transform.GetChild(6).gameObject.SetActive(true);
        else homeScreen.transform.GetChild(6).gameObject.SetActive(false);

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
        if (CurrentTasks.instance.currentTask[index].Quest_ID == 0) return;
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

    private void HideAllInstructions()
    {        
        Transform screen = instructionScreen.transform;
        screen.GetChild(2).gameObject.SetActive(false);
        screen.GetChild(3).gameObject.SetActive(false);
        screen.GetChild(4).gameObject.SetActive(false);
        screen.GetChild(5).gameObject.SetActive(false);
        screen.GetChild(6).gameObject.SetActive(false);
    }

    public void ShowMomText()
    {       
        TogglePDA();
        ShowHome(true);
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
        if (isPause)
        {
            pauseScreen.SetActive(true);
        }
        else
        {
            instructionScreen.SetActive(true);
            HideAllInstructions();
            instructionScreen.transform.GetChild(minigameIndex + 2).gameObject.SetActive(true);
        }        
    }
}
