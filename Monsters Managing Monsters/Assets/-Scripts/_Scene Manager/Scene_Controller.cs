using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Controller : MonoBehaviour {

    public static Scene_Controller instance;

    public GameObject DayOverStats;
    public Animator summaryAnimator;

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

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.PixelMode) return;

        if (DayCycle.instance.BL_ShowResults)
        {
            DayCycle.instance.BL_pause = false;
            DayOverStats.SetActive(true);
            summaryAnimator.SetBool("BL_ShowSummary", true);
        }        
    }

    public void ExitPrompt()
    {
        DayCycle.instance.BL_ShowResults = false;
        summaryAnimator.SetBool("BL_ShowSummary", false);
        Invoke("HideSummary", 1);
        DayCycle.instance.NewDay();        

        PC_Move.BL_canMove = true;
    }

    private void HideSummary()
    {
        DayOverStats.SetActive(false);
    }
}
