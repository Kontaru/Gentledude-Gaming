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
    public float timeOfDay;
}

[RequireComponent(typeof(Light))]
public class DLight : MonoBehaviour {

    public static DLight instance;

    //Our main Dir Light
    private Light mainLight;

    //Spotlights (when it's night time, we might want room lights to turn on?)
    public Light[] spotLights;

    public Daylight[] colorTemperatures;

    public bool DayTime;
    public bool HighNoon;
    public bool Evening;
    public bool Night;

    float currentTime;
    float duration;
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
            duration = 10;
        }
    }
	
	// Update is called once per frame
	void Update () {

        currentTime = DayCycle.instance.DLightCurrentTime();
        duration = DayCycle.instance.timeInDay;
        currentHour = DayCycle.instance.DLightHour();

        DayToNight(currentTime / duration);
	}

    void DayToNight(float step)
    {
        int i = 0;

        foreach (Daylight light in colorTemperatures)
        {
            if (currentHour > light.timeOfDay)
            {
                i++;
            }
        }
        mainLight.intensity = Mathf.Lerp(mainLight.intensity, colorTemperatures[i].brightness, step);
        mainLight.color = Color.Lerp(mainLight.color, colorTemperatures[i].lightCastColour, step);
    }
}
