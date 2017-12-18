using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycle : MonoBehaviour
{

    public static DayCycle instance;

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
    public bool checkForMidnight = false;

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

        if (checkForMidnight)
        {
            if(Time.time >= finalHour)
            {
                beginDay = true;
            }

            //Check if we should bother playing
            if (currentTime >= (timeInDay / dayHours) * workHours)
            {
                pause = true;
                isPlayingGame = false;

                if (currentDay > 3)
                {
                    //SpawnHero
                }
                else
                {
                    if (!ShowResults())
                    {

                    }
                    //Do calculations screen
                    //Do a next day transition
                }
                return;
            }
        }

        if (!pause && isPlayingGame)
        {
            //What is the time right now? (in floats)
            currentTime = Time.time - firstHour;
            //Convert float into something we can use to show the player
            CalculateTime(currentTime);

        }
        else if (!pause && !isPlayingGame)
        {
            //What is the time right now? (in floats)
            currentTime = Time.time * 60 - firstHour;
            //Convert float into something we can use to show the player
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
        checkForMidnight = true;
    }

    //What's the time?
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

    bool ShowResults()
    {
        //Show player scores
        return false;
    }

    public float DLightCurrentTime()
    {
        return currentTime;
    }

    public float DLightHour()
    {
        return displayHour;
    }
}
