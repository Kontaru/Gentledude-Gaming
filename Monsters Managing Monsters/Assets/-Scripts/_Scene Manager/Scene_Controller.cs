using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Controller : MonoBehaviour {

    public static Scene_Controller instance;

    public GameObject DayOverStats;

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

        if (DayCycle.instance.BL_ShowResults)
            StartCoroutine(EndDayEvaluation());
        else
            DayOverStats.SetActive(false);
    }

    public void NextDay()
    {

    }

    IEnumerator EndDayEvaluation()
    {
        DayCycle.instance.BL_pause = false;

        yield return new WaitForSeconds(1);       

        DayOverStats.SetActive(true);        
    }

    public void ExitPrompt()
    {
        DayCycle.instance.NewDay();
        DayCycle.instance.BL_ShowResults = false;

        PC_Move.BL_canMove = true;
    }
}
