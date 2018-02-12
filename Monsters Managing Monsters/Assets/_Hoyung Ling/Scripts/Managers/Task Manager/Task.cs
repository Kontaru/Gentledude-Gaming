using UnityEngine;

[System.Serializable]
public class Step
{
    public string name;                     //This is a name which will display in our quest log. "Find Diana's bracelet!"
    [TextArea(2, 10)]
    public string description;              //Description of my step (how to complete it, what I need for it to be complete)
    public QuestPart requires;
    public GameObject nextStep;
    public bool complete = false;           //Is this step complete?
}

[System.Serializable]
public class Task
{
    [Header("Details")]
    public string name;                     //The name of our task. This should be something easy to distinguish. "Jeff's Quest 1" or "Diana's Bracelet".

    [Header("Accepting a quest")]
    [TextArea(2, 10)]
    public string ST_taskBrief;
    [TextArea(2, 10)]
    public string ST_descriptionDialogue;              //A description of the quest. Please fill this in so everyone else knows what the quest is and what the steps are to completing a quest.
    [TextArea(2, 10)]
    public string ST_acceptedDialogue;
    [TextArea(2, 10)]
    public string ST_declinedDialogue;

    [Header("Finishing a quest")]
    [TextArea(2, 10)]
    public string ST_finishDialogue;
    [TextArea(2, 10)]
    public string ST_failDialogue;
    [TextArea(2, 10)]
    public string ST_waitingDialogue;

    public int Quest_ID;
    public bool inActiveList = false;
    public bool Repeatable = false;

    [HideInInspector] public bool Quest_Complete = false;     //Is our quest complete?
    [HideInInspector] public bool Quest_Fail = false;         //Is our quest complete?
    [HideInInspector] public bool Quest_Finish = false;       //Trigger for NPC hand in

    [HideInInspector] public GameObject GO_belongsTo;

    [Header("Action Points + Motivation")]
    public int IN_actionPointWeight;
    public int IN_motivationAmount;

    [Header("Reward?")]
    public GameObject[] GO_reward;

    [HideInInspector] public bool BL_isObtainable = false;
    [HideInInspector] public bool BL_isAccepted = false;

    private bool BL_Boost = true;

    [Header("Steps")]
    public Step[] Steps;                    //Our steps to completing the quest
    public int step_tracker = 0;
    //Checks if all our steps are complete
    public void StepChecker()
    {
        if (Quest_ID == 0) return;

        if (BL_isAccepted)
        {
            Quest_Complete = true;
            //For each step
            if (step_tracker < Steps.Length)
            {
                Steps[step_tracker].requires.GetComponent<QuestPart>().BL_IsInteractable = true;

                if (Steps[step_tracker].requires.GetComponent<QuestPart>().BL_MinigameComplete)
                {
                    Steps[step_tracker].complete = true;
                    step_tracker++;
                }
            }

            foreach (Step step in Steps)
            {
                //If any of these steps are false, just stop everything and quit the function.
                if (step.complete == false)
                {
                    Quest_Complete = false;
                }
            }
        }

        if (Quest_Finish)
        {
            if (BL_Boost)
            {
                foreach (GameObject produce in GO_reward)
                {
                    if (produce != null) produce.SetActive(true);
                }

                Attribution.Attributes Type = GO_belongsTo.GetComponent<Attribution>().myAttribute;

                if (Quest_Complete)
                    GameManager.instance.PowerBoost(Type, IN_motivationAmount);
                else if (Quest_Fail)
                    GameManager.instance.PowerDeduct(Type, IN_motivationAmount);

                DayCycle.instance.FL_actionPointsUsed += IN_actionPointWeight;
                BL_Boost = false;
            }

            if (Repeatable)
            {
                Quest_Finish = false;
                BL_Boost = false;
            }

            BL_isAccepted = false;
            Quest_Complete = false;
            Quest_Fail = false;
        }
    }

    //public void Obtained()
    //{
    //    if(isAccepted)
    //    {
    //        foreach (Step step in Steps)
    //        {
    //            //If any of these steps are false, just stop everything and quit the function.
    //            if (step.complete == false)
    //            {

    //            }
    //        }
    //    }
    //}
}
