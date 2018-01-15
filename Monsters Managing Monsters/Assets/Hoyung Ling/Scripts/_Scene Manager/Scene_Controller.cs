using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Controller : MonoBehaviour {

    public static Scene_Controller instance;

    public GameObject DayOverStats;

    bool EventOver = false;

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

        if (DayCycle.instance.ShowResults)
        {
            DayOverStats.SetActive(true);
            EndDayEvaluation();
        }else
            DayOverStats.SetActive(false);
        if (DayCycle.instance.EnterHeroes)
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

        if (EventOver)
        {
            DayCycle.instance.NewDay();
            DayCycle.instance.pause = false;
            DayCycle.instance.ShowResults = false;

            PC_Move.canMove = true;
        }
    }

    public void ThirdDay()
    {
        Debug.Log("Event.Hero Entry Event");
        HeroEntry.instance.SpawnHeroes = true;

        if (EventOver)
        {
            DayCycle.instance.NewDay();
            DayCycle.instance.pause = false;
            DayCycle.instance.EnterHeroes = false;

            PC_Move.canMove = true;
        }
    }

    public void ExitPrompt()
    {
        EventOver = true;
    }
}
