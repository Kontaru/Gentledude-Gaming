using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycle : MonoBehaviour
{

    public static DayCycle instance;

    public bool GameStart = false;
    public bool ShowResults = false;
    public bool EnterHeroes = false;

    bool Initialise = true;

    [Header("Day Parameters")]

    //Variables we use to calculate ALL the things
    [Tooltip("Hours in a day")]
    public float workHours;
    float dayHours = 24;
    [Tooltip("How long a day takes irl (minutes)")]
    public float timeInDay;

    //Current time/day for the player
    [Header("Current time/day for the player")]
    public float currentTime;
    public float currentDay;
    float displayHour;
    float displayMinute;

    //Checkpoints in the day
    [Header("Begin day?")]
    public bool beginDay = false;
    public bool pause = false;
    [SerializeField]
    float firstHour;
    [SerializeField]
    float finalHour;

    //i.e: 9am till midnight, player plays the game
    //other hours, simulate time progressing from midnight till 9am at a much faster rate
    [SerializeField]
    bool playerControlledEvent = true;
    [SerializeField]
    bool Daytime = false;
    [SerializeField]
    bool Nighttime = false;

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

    // Update is called once per frame
    void Update()
    {
        if (!GameStart) return;

        if(GameStart && Initialise)
        {
            Initialise = false;
            currentDay++;
            ResetTimer();
        }

        if (beginDay)
        {
            currentDay++;
            beginDay = false;
        }

        if (Daytime)
        {
            Nighttime = false;

            if (!pause && playerControlledEvent)
            {
                Time.timeScale = 1.0f;
                currentTime = Time.time - firstHour;
                CalculateTime(currentTime);
            }

            //Has our day ended?
            if (currentTime + firstHour >= firstHour + (timeInDay / 24) * 16)
            {
                pause = true;
                playerControlledEvent = false;

                Daytime = false;
                Nighttime = true;
            }
        }

        if (Nighttime)
        {
            PC_Move.canMove = false;
            Daytime = false;

            if (!pause && !playerControlledEvent)
            {
                //From midnight until morning, we want to simulate sped up time
                Time.timeScale = 10.0f;
                currentTime = Time.time - firstHour;
                CalculateTime(currentTime);
            }

            if (currentTime + firstHour >= finalHour)
            {
                Daytime = true;
                Nighttime = false;

                ResetTimer();
            }
        }

        if(pause)
        {
            Paused();

            if (currentDay > 3)
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
    }

    void Paused()
    {
        Time.timeScale = 0;
    }

    void ResetTimer()
    {
        if (currentDay < 10)
            Days.text = string.Format("0" + currentDay);
        else
            Days.text = string.Format("" + currentDay);

        firstHour = Time.time;
        finalHour = Time.time + timeInDay;
        currentTime = 0;

        pause = false;
        Daytime = true;
        playerControlledEvent = true;
    }

    //Convert float into something we can use to show the player
    void CalculateTime(float time)
    {
        //Current time * time it takes for an hour
        displayHour = (24 - workHours) + Mathf.Floor(time * dayHours / timeInDay);  //What is the time right now? (in hours)

        if(displayHour > 23)
            Hour.text = string.Format("0" + (displayHour - 24));
        else if (displayHour < 10)
            Hour.text = string.Format("0" + displayHour);
        else
            Hour.text = string.Format("" + displayHour);


        //Current time * time it takes for a minute - (the hour that we're at * 60)
        displayMinute = Mathf.Floor(time * 60 * dayHours / timeInDay - (60 * (displayHour - (24 - workHours))));

        if (displayMinute < 10)
            Minute.text = string.Format("0" + displayMinute);
        else
            Minute.text = string.Format("" + displayMinute);
    }

    //Some return values for DLight

    public float FL_CurrentTime()
    {
        return currentTime;
    }

    public float FL_CurrentHour()
    {
        return (24 - workHours) + (currentTime * dayHours / timeInDay);
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

    public bool BL_Daytime()
    {
        return Daytime;
    }

    public void NewDay()
    {
        beginDay = true;
    }
}
