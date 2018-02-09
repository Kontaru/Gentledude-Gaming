using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Controller : MonoBehaviour {

    public static Scene_Controller instance;

    public GameObject DayOverStats;

    bool BL_EventOver = false;

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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.PixelMode) return;

        if (DayCycle.instance.BL_ShowResults)
        {
            DayOverStats.SetActive(true);
            EndDayEvaluation();
        }else
            DayOverStats.SetActive(false);
        if (DayCycle.instance.BL_EnterHeroes)
        {
            ThirdDay();
        }
    }

    public void NextDay()
    {

    }

    public void EndDayEvaluation()
    {
        Debug.Log("Event.Player Evaluation");

        if (BL_EventOver)
        {
            DayCycle.instance.NewDay();
            DayCycle.instance.BL_pause = false;
            DayCycle.instance.BL_ShowResults = false;

            PC_Move.BL_canMove = true;
        }
    }

    public void ThirdDay()
    {
        Debug.Log("Event.Hero Entry Event");
        HeroEntry.instance.SpawnHeroes = true;

        if (BL_EventOver)
        {
            DayCycle.instance.NewDay();
            DayCycle.instance.BL_pause = false;
            DayCycle.instance.BL_EnterHeroes = false;

            PC_Move.BL_canMove = true;
        }
    }

    public void ExitPrompt()
    {
        BL_EventOver = true;
    }
}
