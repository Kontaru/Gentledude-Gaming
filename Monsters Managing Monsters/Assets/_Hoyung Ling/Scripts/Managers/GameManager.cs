using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject Player;
    private GameObject notification;
    private Text questText;

    public bool PixelMode = false;

    [Header("Controls")]

    public KeyCode KC_CameraLock;
    public KeyCode KC_Interact;

    [Header("Movement")]
    public KeyCode KC_Up;
    public KeyCode KC_Left;
    public KeyCode KC_Down;
    public KeyCode KC_Right;

    public int day;
    public int score;

    [Header("Monster Stats")]
    public int PL_Finance = 0;
    public int PL_HR = 0;
    public int PL_IT = 0;
    public int PL_Janitorial = 0;
    public int PL_Marketing = 0;
    public int PL_Overseas = 0;
    public int PL_Security = 0;

    private Animator animator;

    bool BL_Pause = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        notification = GameObject.Find("QuestCompletePanel");
        animator = notification.GetComponentInChildren<Animator>();
    }

    public void PowerBoost(Attribution.Attributes attr, int amount)
    {
        if (attr == Attribution.Attributes.Finance)
            PL_Finance += amount;
        if (attr == Attribution.Attributes.HR)
            PL_HR += amount;
        if (attr == Attribution.Attributes.IT)
            PL_IT += amount;
        if (attr == Attribution.Attributes.Janitorial)
            PL_Janitorial += amount;
        if (attr == Attribution.Attributes.Marketing)
            PL_Marketing += amount;
        if (attr == Attribution.Attributes.Overseas)
            PL_Overseas += amount;
        if (attr == Attribution.Attributes.Security)
            PL_Security += amount;

    }

    public void QuestCompleted(string questName, GameObject plusObject)
    {
        questText.text = questName;
        StartCoroutine(ShowNotification(plusObject));        
    }

    IEnumerator ShowNotification(GameObject GO)
    {
        GO.SetActive(true);
        animator.SetBool("BL_ShowNotification", true);
        yield return new WaitForSeconds(4);
        GO.SetActive(false);
        animator.SetBool("BL_ShowNotification", false);
    }

    #region ~ Scene Related ~

    public void NextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex < 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    public void LastScene()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        BL_Pause = !BL_Pause;

        if (BL_Pause == true)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    #endregion
}
