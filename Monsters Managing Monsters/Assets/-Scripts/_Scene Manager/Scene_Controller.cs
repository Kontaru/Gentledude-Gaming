using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Controller : MonoBehaviour {

    public static Scene_Controller instance;

    public GameObject DayOverStats;
    public Animator summaryAnimator;
    public Transform spawnPoint;
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
        DayCycle.instance.BL_ShowResults = false;
        summaryAnimator.SetBool("BL_ShowSummary", false);
        Invoke("HideSummary", 1);

        DayCycle.instance.NewDay();
        GameManager.instance.Player.transform.position = spawnPoint.position;
        
        BL_firstFlag = false;
        PC_Move.BL_canMove = true;
    }

    private void HideSummary()
    {
        DayOverStats.SetActive(false);
    }
}
