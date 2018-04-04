using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycle : MonoBehaviour
{

    public static DayCycle instance;

    public bool BL_ShowResults = false;
    public bool BL_EnterHeroes = false;

    //Variables we use to calculate the current state in the day
    float FL_workHours = 16;
    float FL_dayHours = 24;
    float FL_displayHour;
    float FL_displayMinute;

    //Night hours (in action points)
    float FL_time;
    float FL_morning;

    [Tooltip("Action Points")]
    public float FL_actionPointsAvailable;
    public float FL_actionPointsUsed;
    public int IN_currentDay;


    //Checkpoints in the day
    [Header("Begin day?")]
    public int IN_DaysInWeek = 6;
    public bool BL_beginDay = false;
    public bool BL_pause = false;

    //i.e: 9am till midnight, player plays the game
    //other hours, simulate time progressing from midnight till 9am at a much faster rate
    [SerializeField]
    bool BL_playerControlledEvent = true;

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
    public Text AP;

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

        if (BL_beginDay)
        {
            FL_actionPointsUsed = 0;
            IN_currentDay++;
            ResetTimer();
            BL_beginDay = false;
        }

        if (State == TimeState.Daytime) DayTimeIterationCycle();
        else if (State == TimeState.Nighttime) NightTimeIteractionCycle();
        
        if(BL_pause)
        {
            Paused();
        }
    }

    void Paused()
    {
        Time.timeScale = 0;

        if (IN_currentDay > IN_DaysInWeek)
        {
            //SpawnHero
            BL_EnterHeroes = true;
            BL_ShowResults = false;
        }
        else
        {
            //Do calculations screen
            BL_EnterHeroes = false;
            BL_ShowResults = true;
        }
    }

    #region --- Day Cycles ---

    void DayTimeIterationCycle()
    {
        if (!BL_pause && BL_playerControlledEvent)
        {
            Time.timeScale = 1.0f;
            CalculateTime(FL_actionPointsUsed);
        }

        CheckForNightTime();
    }

    void CheckForNightTime()
    {
        //Has our day ended?
        if (FL_actionPointsUsed >= FL_actionPointsAvailable)
        {
            FL_actionPointsUsed = FL_actionPointsAvailable;
            FL_morning = FL_actionPointsAvailable * FL_dayHours / FL_workHours;
            FL_time = FL_actionPointsAvailable;

            BL_pause = true;
            BL_playerControlledEvent = false;

            State = TimeState.Nighttime;
        }
    }

    void NightTimeIteractionCycle()
    {

        if (!BL_pause && !BL_playerControlledEvent)
        {
            //From midnight until morning, we want to simulate sped up time
            Time.timeScale = 2.0f;
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
        if (IN_currentDay < 10)
            Days.text = string.Format("0" + IN_currentDay);
        else
            Days.text = string.Format("" + IN_currentDay);        

        FL_actionPointsUsed = 0;

        BL_pause = false;
        State = TimeState.Daytime;
        BL_playerControlledEvent = true;
    }

    //Convert float into something we can use to show the player
    void CalculateTime(float time)
    {
        //Current time * time it takes for an hour
        FL_displayHour = (24 - FL_workHours) + 
            Mathf.Floor(time * 
            (FL_workHours /  FL_actionPointsAvailable)
            );  //What is the time right now? (in hours)

        if(FL_displayHour > 23)
            Hour.text = string.Format("0" + (FL_displayHour - 24));
        else if (FL_displayHour < 10)
            Hour.text = string.Format("0" + FL_displayHour);
        else
            Hour.text = string.Format("" + FL_displayHour);

        if (FL_actionPointsAvailable < 10)
            AP.text = string.Format("0" + time);
        else
            AP.text = string.Format("" + time);

        //Current time * time it takes for a minute - (the hour that we're at * 60)
        FL_displayMinute = Mathf.Floor(time * 60 *
            (FL_workHours / FL_actionPointsAvailable)
            - (60 * (FL_displayHour - (24 - FL_workHours))));

        if (FL_displayMinute < 10)
            Minute.text = string.Format("0" + FL_displayMinute);
        else
            Minute.text = string.Format("" + FL_displayMinute);
    }

    #region --- Some return values for DLight ---

    public float FL_CurrentTime()
    {
        return FL_actionPointsUsed;
    }

    public float FL_CurrentHour()
    {
        return (24 - FL_workHours) + (FL_actionPointsUsed * FL_dayHours / FL_actionPointsAvailable);
    }

    public float[] FL_TheTime()
    {
        float[] thyme = new float[2];
        thyme[0] = FL_displayHour;
        thyme[1] = FL_displayMinute;

        return thyme;
    }

    public bool BL_Paused()
    {
        return BL_pause;
    }

    #endregion

    public void NewDay()
    {
        BL_beginDay = true;
    }
}
