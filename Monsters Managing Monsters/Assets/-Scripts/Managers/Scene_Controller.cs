using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Scene_Controller : MonoBehaviour {

    public static Scene_Controller instance;

    public GameObject DayOverStats;
    public Animator summaryAnimator;
    public ScrollRect summaryRect;
    private bool BL_firstFlag;

    #region Typical Singleton Format

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion

    void Start()
    {
        HideSummary();
    }

    void Update()
    {
        if (GameManager.instance.PixelMode) return;

        if (DayCycle.instance.BL_ShowResults)
        {
            DayCycle.instance.BL_pause = false;
            DayOverStats.SetActive(true);
            summaryAnimator.SetBool("BL_ShowSummary", true);
            if (!BL_firstFlag)
            {
                EndDaySummary.instance.CalculateScores();
                BL_firstFlag = true;
            }            
        }        
    }

    public void ExitPrompt()
    {
        EndDaySummary summary = EndDaySummary.instance;

        if (GameManager.instance.BL_GameOver)
        {            
            GameManager.instance.LoadScene(3);

        }
        else
        {
            DayCycle.instance.BL_ShowResults = false;
            summaryAnimator.SetBool("BL_ShowSummary", false);
            Invoke("HideSummary", 1);

            DayCycle.instance.NewDay();
            OfficeEntrance.instance.RespawnPlayer();

            BL_firstFlag = false;
            PC_Move.BL_canMove = true;
        }        
    }

    private void HideSummary()
    {
        summaryRect.verticalNormalizedPosition = 1;
        DayOverStats.SetActive(false);
    }
}
