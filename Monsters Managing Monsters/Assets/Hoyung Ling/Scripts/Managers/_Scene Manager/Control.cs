using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    public static Control instance;

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
        if (DayCycle.instance.ShowResults)
            EndDayEvaluation();
        if (DayCycle.instance.EnterHeroes)
            HeroEntry.instance.SpawnHeroes = true;
	}

    public void BeginDay()
    {

    }

    public void EndDayEvaluation()
    {
        
    }
}
