using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycle : MonoBehaviour
{

    public static DayCycle instance;

    public bool ShowResults = false;
    public bool EnterHeroes = false;

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
    bool begunDay = false;
    bool pause = false;
    float firstHour;
    float finalHour;

    //Hours of the day that the player is actually playing
    //When false, it should represent the hours in the day that the player doesn't play
    //i.e: midnight till 9am
    public bool isPlayingGame = true;
    public bool daytimeLoop = false;

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
        if (beginDay)
        {
            NewDay();
            beginDay = false;
        }

        if (!begunDay) return;

        if (daytimeLoop)
        {
            //Has our day ended?
            if (currentTime >= (timeInDay / dayHours) * workHours)
            {
                pause = true;
                isPlayingGame = false;

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

            if (!pause && isPlayingGame)
            {
                currentTime = Time.time - firstHour;
                CalculateTime(currentTime);
            }
        }


        else if (!pause && !isPlayingGame)
        {
            //From midnight until morning, we want to simulate sped up time
            currentTime = Time.time * 60 - firstHour;
            CalculateTime(currentTime);
        }
    }

    //Begin a new day!
    void NewDay()
    {
        currentDay++;

        if (currentDay < 10)
            Days.text = string.Format("0" + currentDay);
        else
            Days.text = string.Format("" + currentDay);

        firstHour = Time.time;
        finalHour = Time.time + timeInDay;

        pause = false;
        begunDay = true;
        daytimeLoop = true;
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

    public float DLightCurrentTime()
    {
        return currentTime;
    }

    public float DLightHour()
    {
        return (24 - workHours) + (currentTime * dayHours / timeInDay);
    }
}
