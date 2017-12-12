using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycle : MonoBehaviour {

    public static DayCycle instance;

    [Header("Day Parameters")]
    [Tooltip("Hours in a day")]
    public float dayHours;
    [Tooltip("How long a day takes irl (minutes)")]
    public float realTime;
    public float currentDay;
    float firstHour;
    float finalHour;
    bool beginDay;

    [Header("UI")]
    public Text Hour;
    public Text Minute;

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
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (beginDay)
        {
            NewDay();
        }

        if(Time.time == finalHour)
        {
            currentDay++;
            if(currentDay > 3)
            {
                //SpawnHero
            }else
            {
                //Do a next day transition
            }
        }


	}

    //Begin a new day!
    void NewDay()
    {
        firstHour = Time.time;
        finalHour = Time.time + realTime;
    }
}
