using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class HeroEntry : MonoBehaviour {

    public static HeroEntry instance;
    public HeroMinigame CurrentHero;

    //----- Some important variables

    public bool SpawnHeroes;
    public bool BL_HeroArrival;
    public string ST_heroArrival;
    public string ST_heroDeparture;

    private bool BL_Confirmed;

    private GameObject monster;
    private Vector3 delta;

    public Flowchart flowchart;

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
    void Update () {

        if (GameManager.instance.PixelMode) return;

        if (Input.GetKeyDown(KeyCode.Q) || DayCycle.instance.IN_currentDay == DayCycle.instance.IN_DaysInWeek)
        {
            InitialiseData();
            HeroArrival();
            StartMinigame();
            HeroDeparture();
        }
    }

    private void InitialiseData()
    {
        CurrentHero = HeroMinigameManager.instance.Upcoming;

    }

    private void HeroArrival()
    {
        CurrentHero.hero.SetActive(true);
        CameraFollow.otherLook = CurrentHero.hero;

        StartCoroutine(StartDialogue(ST_heroArrival));        
    }

    private void HeroDeparture()
    {
        CameraFollow.otherLook = null;
    }

    public void StartMinigame()
    {
       //Fade
    }

    IEnumerator StartDialogue(string text)
    {
        yield return new WaitForSeconds(2);
        flowchart.SetStringVariable("hero_entry", ST_heroArrival);
        Fungus.Flowchart.BroadcastFungusMessage("Entry");
    }

    /*private void MonsterPlacement()
    {
        if (BL_Confirmed) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag != "Monster") return;

            if (Input.GetMouseButtonDown(0))
            {
                monster = hit.transform.gameObject;                
                delta = monster.transform.position - hit.point;
                DropHighlight(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                DropHighlight(false);
                monster = null;
            }

            if (Input.GetMouseButton(0))
            {
                if (monster != null)
                {
                    monster.transform.position = new Vector3(hit.point.x + delta.x, monster.transform.position.y + 5, hit.point.z + delta.z);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmationScreen();
        }
    }

    private void DropHighlight(bool flag)
    {
        GameObject parent = monster.transform.Find("Graphic").gameObject;
        GameObject highlight = parent.transform.GetChild(1).gameObject;

        if (flag) highlight.SetActive(true);
        else highlight.SetActive(false);
    }

    private void ConfirmationScreen()
    {
        Debug.Log("Confirm monster positions?");
        //BL_Confirmed = true;
    }*/
}
