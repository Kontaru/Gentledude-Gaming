using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class HeroEntry : MonoBehaviour {

    public static HeroEntry instance;
    public HeroMinigame CurrentHero;

    public Camera minigameCam;
    public Image fadeImage;

    #region    //----- Hero Variables (Initialised by code) (No touchy)

    private GameObject hero;
    private GameObject minigame;
    private string ST_entryText;
    private string ST_playerWin;
    private string ST_playerDefeat;

    #endregion

    public enum InteractionState
    {
        Enter,
        Minigame,
        Exit,
        End
    }

    public InteractionState CurrentState;

    //Auto handled
    public bool BL_EndInteraction = false;
    public bool BL_playerWin = false;     //Dean SET THIS FLAGS APPROPRIATELY
    private bool BL_Converse = true;

    [HideInInspector]
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
    void Update() {

        if (GameManager.instance.PixelMode) return;

        if (Input.GetKeyDown(KeyCode.Q) || DayCycle.instance.IN_currentDay == DayCycle.instance.IN_DaysInWeek)
        {
            if (CurrentState != InteractionState.End)
            {
                InitialiseData();
                InteractionLoop();
            }
            else
            {
                //Will then assign motivation boosts appropriately
                BL_EndInteraction = true;
                //EndDaySummary card
            }

            UpdateFlags();
        }

        if (Input.GetKeyDown(KeyCode.K)) StartMinigame();
    }

    #region Interaction Loop (Initialise Data, Do Loop, Update Flag to HeroMinigameManager)

    //Interaction loop for hero
    private void InteractionLoop()
    {
        if (CurrentState == InteractionState.Enter) HeroArrival();
        else if (CurrentState == InteractionState.Minigame) StartMinigame();
        else if (CurrentState == InteractionState.Exit) HeroDeparture();
    }

    private void InitialiseData()
    {
        CurrentHero = HeroMinigameManager.instance.Upcoming;

        hero = CurrentHero.hero;
        minigame = CurrentHero.minigame;
        ST_entryText = CurrentHero.ST_heroEntry;
        ST_playerWin = CurrentHero.ST_heroDefeat;
        ST_playerDefeat = CurrentHero.ST_heroWin;
    }

    private void UpdateFlags()
    {
        CurrentHero.Quest_playerWin = BL_playerWin;
        CurrentHero.Quest_Finish = BL_EndInteraction;
    }

    #endregion

    #region States (Arrival, Minigame, Departure)

    //When the hero arrives
    private void HeroArrival()
    {
        hero.SetActive(true);
        if (BL_Converse)
        {
            StartCoroutine(Entry());
            BL_Converse = false;
        }
    }

    private void HeroDeparture()
    {
        if (BL_Converse)
        {
            StartCoroutine(Conclusion());
            BL_Converse = false;
        }
    }

    //Dean MINIGAME STUFF HERE
    public void StartMinigame()
    {
        bool BL_MinigameEnd = false;
        
        StartCoroutine(FadeEffect(2, fadeImage));        

        if(BL_MinigameEnd)
            CurrentState = InteractionState.Exit;
    }

    #endregion

    #region Coroutines

    IEnumerator Entry()
    {
        CameraFollow.instance.otherLook = hero;
        yield return new WaitForSeconds(2);
        flowchart.SetStringVariable("hero_entry", ST_entryText);
        Fungus.Flowchart.BroadcastFungusMessage("Entry");
        if(flowchart.GetBooleanVariable("bl_textCycleOver") == true)
        {
            CurrentState = InteractionState.Minigame;
            BL_Converse = true;
            flowchart.SetBooleanVariable("bl_textCycleOver", false);
        }
    }

    IEnumerator Conclusion()
    {
        yield return new WaitForSeconds(2);
        if(BL_playerWin)
            flowchart.SetStringVariable("hero_defeat", ST_playerWin);
        else
            flowchart.SetStringVariable("hero_defeat", ST_playerDefeat);

        Fungus.Flowchart.BroadcastFungusMessage("Exit");

        if (flowchart.GetBooleanVariable("bl_textCycleOver") == true)
        {
            CurrentState = InteractionState.End;
            BL_Converse = true;
            CameraFollow.instance.otherLook = null;
            flowchart.SetBooleanVariable("bl_textCycleOver", false);
        }
    }

    IEnumerator FadeEffect(float t, Image i)
    {        
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }

        yield return new WaitForSeconds(2);

        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }

        minigameCam.tag = "MainCamera";
    }

    #endregion






















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
