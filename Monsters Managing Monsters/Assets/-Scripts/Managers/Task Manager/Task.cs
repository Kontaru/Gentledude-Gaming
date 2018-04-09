using UnityEngine;

[System.Serializable]
public class Step
{
    public string name;                     //This is a name which will display in our quest log. "Find Diana's bracelet!"
    [TextArea(2, 10)]
    public string description;              //Description of my step (how to complete it, what I need for it to be complete)
    public QuestPart requires;
    public bool TurnOff = false;
    public bool Hidden = false;
    public bool complete = false;           //Is this step complete?
    public bool active = false;
}

[System.Serializable]
public class Task
{
    public string name;
    public Quests Quest;

    [Header("Quest States")]
    public bool Quest_Complete = false;     //Is our quest complete?
    public bool Quest_Fail = false;         //Is our quest complete?
    public bool Quest_Finish = false;       //Trigger for NPC hand in

    public bool BL_isObtainable = false;
    public bool BL_isAccepted = false;

    [Header("In Use?")]
    public bool inActiveList = false;

    #region Internal Stuff - Not important

    [HideInInspector] public string ST_taskBrief;
    [HideInInspector] public string ST_descriptionDialogue;              //A description of the quest. Please fill this in so everyone else knows what the quest is and what the steps are to completing a quest.
    [HideInInspector] public string ST_acceptedDialogue;
    [HideInInspector] public string ST_declinedDialogue;

    [HideInInspector] public string ST_finishDialogue;
    [HideInInspector] public string ST_failDialogue;
    [HideInInspector] public string ST_waitingDialogue;

    [HideInInspector] public int Quest_ID;
    [HideInInspector] public bool Repeatable = false;

    [HideInInspector] public int IN_actionPointWeight;
    [HideInInspector] public int IN_motivationAmount;

    #endregion

    [Header("Quest Requirements")]
    public int step_tracker = 0;
    public Step[] Steps;                    //Our steps to completing the quest
    public GameObject GO_belongsTo;

    private bool BL_firstFlag = false;
    private bool BL_Boost = true;

    //Checks if all our steps are complete
    public void StepChecker()
    {
        if (Quest_ID == 0) return;       

        if (BL_isAccepted)
        {
            FirstFlag();

            Quest_Complete = true;
            //For each step
            if (step_tracker < Steps.Length)
            {
                foreach (Step step in Steps)
                {
                    //If any of these steps are false, just stop everything and quit the function.
                    if (step.complete == false)
                    {
                        if (step.TurnOff && step.active == false)
                            step.requires.gameObject.SetActive(false);

                        Quest_Complete = false;
                    }
                }

                if (!Quest_Complete)
                {
                    StepStatus();
                }
            }
        }

        CheckFinished();
    }
    
    void StepStatus()
    {
        if (Steps[step_tracker].TurnOff)
            Steps[step_tracker].requires.gameObject.SetActive(true);

        Steps[step_tracker].active = true;

        QuestPart questStep = Steps[step_tracker].requires.GetComponent<QuestPart>();

        questStep.BL_IsInteractable = true;

        if (questStep.BL_MinigameComplete)
        {
            Steps[step_tracker].complete = true;
            questStep.BL_IsInteractable = false;
            questStep.BL_MinigameComplete = false;

            if (questStep.BL_MinigameFail)
                Quest_Fail = true;


            Steps[step_tracker].active = false;
            step_tracker += 1;
        }
    }

    void FirstFlag()
    {
        if (!BL_firstFlag)
        {
            EndDaySummary.instance.QuestGained(name);
            BL_firstFlag = true;
        }
    }

    void CheckFinished()
    {
        if (Quest_Finish || Quest_Fail)
        {

            if (BL_firstFlag)
            {
                EndDaySummary.instance.tasksCount++;
                EndDaySummary.instance.QuestCompleted(name, Quest_Fail);
                BL_firstFlag = false;
            }

            if (BL_Boost)
            {
                Attribution.Attributes Type = GO_belongsTo.GetComponent<Attribution>().myAttribute;

                if (!Quest_Fail)
                    GameManager.instance.PowerBoost(Type, IN_motivationAmount);

                DayCycle.instance.FL_actionPointsUsed += IN_actionPointWeight;
                BL_Boost = false;
            }

            foreach (Step step in Steps)
            {
                step.complete = false;
                step.active = false;
            }

            Quest_Fail = false;

            if (Repeatable)
            {
                Quest_Finish = false;
            }
            
        }
    }

    public void InitialiseQuest()
    {
        name = Quest.name;

        ST_taskBrief = Quest.TaskBrief;
        ST_descriptionDialogue = Quest.DescriptionDialogue;
        ST_acceptedDialogue = Quest.AcceptedDialogue;
        ST_declinedDialogue = Quest.DeclinedDialogue;

        ST_finishDialogue = Quest.FinishDialogue;
        ST_failDialogue = Quest.FailDialogue;
        ST_waitingDialogue = Quest.WaitingDialogue;

        Quest_ID = Quest.Quest_ID;
        Repeatable = Quest.Repeatable;

        if (Quest.Presets == Quests.Values.None)
        {
            IN_actionPointWeight = Quest.customActionPointWeight;
            IN_motivationAmount = Quest.customMotivationAmount;
        }else if (Quest.Presets == Quests.Values.Level_01)
        {
            IN_actionPointWeight = 3;
            IN_motivationAmount = 100;
        }
        else if (Quest.Presets == Quests.Values.Level_02)
        {
            IN_actionPointWeight = 6;
            IN_motivationAmount = 220;
        }
        else if (Quest.Presets == Quests.Values.Level_03)
        {
            IN_actionPointWeight = 10;
            IN_motivationAmount = 500;
        }

    }
}
