using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycle : MonoBehaviour
{

    public static DayCycle instance;

    public bool ShowResults = false;
    public bool EnterHeroes = false;

    //Variables we use to calculate the current state in the day
    float workHours = 16;
    float dayHours = 24;
    float displayHour;
    float displayMinute;

    //Night hours (in action points)
    float FL_time;
    float FL_morning;

    [Tooltip("Action Points")]
    public float actionPointsAvailable;
    public static float actionPointsUsed;
    public float currentDay;


    //Checkpoints in the day
    [Header("Begin day?")]
    public bool beginDay = false;
    public bool pause = false;

    //i.e: 9am till midnight, player plays the game
    //other hours, simulate time progressing from midnight till 9am at a much faster rate
    [SerializeField]
    bool playerControlledEvent = true;

    public enum TimeState
    {
        None,
        Daytime,
        Nighttime,
        Paused
    }

    public TimeState State;

    [Header("UI")]
    public Text Hour;
    public Text Minute;
    public Text Days;

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
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.PixelMode) return;

        if (beginDay)
        {
            currentDay++;
            beginDay = false;
        }

        if (State == TimeState.Daytime) DayTimeIterationCycle();
        else if (State == TimeState.Nighttime) NightTimeIteractionCycle();
        
        if(pause)
        {
            Paused();
        }
    }

    void Paused()
    {
        Time.timeScale = 0;

        if (currentDay > 5)
        {
            //SpawnHero
            EnterHeroes = true;
            ShowResults = false;
        }
        else
        {
            //Do calculations screen
            EnterHeroes = false;
            ShowResults = true;
        }
    }

    #region --- Day Cycles ---

    void DayTimeIterationCycle()
    {
        if (!pause && playerControlledEvent)
        {
            Time.timeScale = 1.0f;
            CalculateTime(actionPointsUsed);
        }

        CheckForNightTime();
    }

    void CheckForNightTime()
    {
        //Has our day ended?
        if (actionPointsUsed >= actionPointsAvailable)
        {
            actionPointsUsed = actionPointsAvailable;
            FL_morning = actionPointsAvailable * dayHours / workHours;
            FL_time = actionPointsAvailable;

            pause = true;
            playerControlledEvent = false;

            State = TimeState.Nighttime;
        }
    }

    void NightTimeIteractionCycle()
    {

        if (!pause && !playerControlledEvent)
        {
            //From midnight until morning, we want to simulate sped up time
            Time.timeScale = 10.0f;
            FL_time++;
            CalculateTime(FL_time);
        }

        CheckForDayTime();
    }

    void CheckForDayTime()
    {
        if (FL_time >= FL_morning)
            ResetTimer();
    }

    #endregion

    void ResetTimer()
    {
        if (currentDay < 10)
            Days.text = string.Format("0" + currentDay);
        else
            Days.text = string.Format("" + currentDay);

        actionPointsUsed = 0;

        pause = false;
        State = TimeState.Daytime;
        playerControlledEvent = true;
    }

    //Convert float into something we can use to show the player
    void CalculateTime(float time)
    {
        //Current time * time it takes for an hour
        displayHour = (24 - workHours) + 
            Mathf.Floor(time * 
            (workHours /  actionPointsAvailable)
            );  //What is the time right now? (in hours)

        if(displayHour > 23)
            Hour.text = string.Format("0" + (displayHour - 24));
        else if (displayHour < 10)
            Hour.text = string.Format("0" + displayHour);
        else
            Hour.text = string.Format("" + displayHour);


        //Current time * time it takes for a minute - (the hour that we're at * 60)
        displayMinute = Mathf.Floor(time * 60 *
            (workHours / actionPointsAvailable)
            - (60 * (displayHour - (24 - workHours))));

        if (displayMinute < 10)
            Minute.text = string.Format("0" + displayMinute);
        else
            Minute.text = string.Format("" + displayMinute);
    }

    #region --- Some return values for DLight ---

    public float FL_CurrentTime()
    {
        return actionPointsUsed;
    }

    public float FL_CurrentHour()
    {
        return (24 - workHours) + (actionPointsUsed * dayHours / actionPointsAvailable);
    }

    public float[] FL_TheTime()
    {
        float[] thyme = new float[2];
        thyme[0] = displayHour;
        thyme[1] = displayMinute;

        return thyme;
    }

    public bool BL_Paused()
    {
        return pause;
    }

    #endregion

    public void NewDay()
    {
        beginDay = true;
    }
}
