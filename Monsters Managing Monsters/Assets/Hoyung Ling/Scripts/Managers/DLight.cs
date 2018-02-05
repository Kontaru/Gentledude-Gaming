using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Daylight
{
    public string name;
    public Color lightCastColour;

    [Range(0, 1)]
    public float brightness;
    public float when;
}

[RequireComponent(typeof(Light))]
public class DLight : MonoBehaviour {

    public static DLight instance;

    //Our main Dir Light
    private Light mainLight;

    //Spotlights (when it's night time, we might want room lights to turn on?)
    public Light[] spotLights;

    public Daylight[] skyColour;

    const int midnight = 24;
    public int morningStart;
    public int afternoonStart;
    public int eveningStart;
    public int nightStart;    

    int currentStep = 0;
    float until = 0;
    float duration = 0;
    float rate = 0;

    float currentTime;
    float timeInDay;
    float currentHour;

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

        mainLight = GetComponent<Light>();
        if (DayCycle.instance == null)
        {
            currentTime = 0;
            timeInDay = 10;
        }
    }
	
	// Update is called once per frame
	void Update () {

        currentTime = DayCycle.instance.FL_CurrentTime();
        timeInDay = DayCycle.instance.actionPointsAvailable;
        currentHour = DayCycle.instance.FL_CurrentHour();

        UpdateLightCycle(timeInDay);
	}

    void UpdateLightCycle(float realTime)
    {
        if (currentHour > morningStart && currentHour < afternoonStart) currentStep = 0;
        else if (currentHour > afternoonStart && currentHour < eveningStart) currentStep = 1;
        else if (currentHour > eveningStart && currentHour < nightStart) currentStep = 2;
        else if (currentHour > nightStart && currentHour < midnight) currentStep = 3;

        if (currentStep == skyColour.Length - 1) until = 24;
        else until = skyColour[currentStep + 1].when;

        duration = until - skyColour[currentStep].when;
        rate = 24 / (24 / 16) / realTime;
        duration = rate * duration;

        mainLight.intensity = Mathf.Lerp(mainLight.intensity, skyColour[currentStep].brightness, Time.deltaTime / duration);
        mainLight.color = Color.Lerp(mainLight.color, skyColour[currentStep].lightCastColour, Time.deltaTime / duration);
    }
}
