using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroMinigame
{
    [Header("Details")]
    public string name;                     //The name of our task. This should be something easy to distinguish. "Jeff's Quest 1" or "Diana's Bracelet".
    public GameObject hero;
    public GameObject minigame;

    [Header("Hero Dialogue")]
    [TextArea(2, 10)]
    public string ST_heroEntry;
    [TextArea(2, 10)]
    public string ST_heroDefeat;
    [TextArea(2, 10)]
    public string ST_heroWin;

    [Range(1, 3)]
    public int rating;

    [HideInInspector] public bool Quest_Complete = false;     //Is our quest complete?
    [HideInInspector] public bool Quest_Fail = false;         //Is our quest complete?
    [HideInInspector] public bool Quest_Finish = false;       //Trigger for NPC hand in
    [HideInInspector] public bool BL_isAccepted = false;



    [Header("Action Points + Motivation")]
    int IN_motivationAmount;

    private bool BL_Boost = true;

    //Checks if all our steps are complete
    public void CheckFinish()
    {
        if (Quest_Finish && Quest_Complete)
        {
            Quest_Complete = false;

            if (BL_Boost)
            {
                IN_motivationAmount = rating * 100;
                GameManager.instance.PowerBoostAll(IN_motivationAmount);
                BL_Boost = false;
            }
        }

        if (Quest_Finish && Quest_Fail)
        {
            Quest_Fail = false;

            if (BL_Boost)
            {
                IN_motivationAmount = rating * 10;
                GameManager.instance.PowerDeductAll(IN_motivationAmount);
                BL_Boost = false;
            }
        }
    }
}

public class HeroMinigameManager : MonoBehaviour {

    public HeroMinigame[] Minigames;                //A collection of tasks
    public HeroMinigame Upcoming;
    int minigameCount = 0;

    public static HeroMinigameManager instance;

    #region Typical Singleton Format

    void Awake()
    {
        //Singleton stuff
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion

    void Start()
    {
        Randomise();
        Upcoming = Minigames[minigameCount];
    }

    void Update()
    {
        Upcoming.CheckFinish();

        if (Upcoming.Quest_Finish)
        {
            minigameCount++;
            Upcoming = Minigames[minigameCount];
        }
    }

    void Randomise()
    {

    }
}
