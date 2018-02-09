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
    public string taskBrief;
    [TextArea(2, 10)]
    public string descriptionDialogue;              //A description of the quest. Please fill this in so everyone else knows what the quest is and what the steps are to completing a quest.
    [TextArea(2, 10)]
    public string acceptedDialogue;
    [TextArea(2, 10)]
    public string declinedDialogue;

    [Header("Finishing a quest")]
    [TextArea(2, 10)]
    public string finishDialogue;
    [TextArea(2, 10)]
    public string failDialogue;
    [TextArea(2, 10)]
    public string waitingDialogue;

    public int QuestID;
    public bool inActiveList = false;
    public bool Repeatable = false;

    [HideInInspector] public bool QuestComplete = false;     //Is our quest complete?
    [HideInInspector] public bool QuestFail = false;     //Is our quest complete?
    [HideInInspector] public bool QuestFinish = false;       //Trigger for NPC hand in

    [HideInInspector] public GameObject belongsTo;

    [Header("Action Points + Motivation")]
    public int actionPointWeight;
    public int motivationAmount;

    [Header("Reward?")]
    public GameObject[] reward;

    [HideInInspector] public bool isObtainable = false;
    public bool isAccepted = false;

    private bool BL_Boost = true;

    [Header("Steps")]
    public Step[] Steps;                    //Our steps to completing the quest
    public int step_tracker = 0;
    //Checks if all our steps are complete
    public void StepChecker()
    {
        if (QuestID == -1) return;

        if (isAccepted)
        {
            QuestComplete = true;
            //For each step
            if (step_tracker < Steps.Length)
            {
                Steps[step_tracker].requires.gameObject.SetActive(true);
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
                    QuestComplete = false;
                }
            }
        }else
        {
            //For each step
            foreach (Step step in Steps)
            {
                step.requires.gameObject.SetActive(false);
            }
        }

        if (QuestFinish || QuestFail)
        {
            QuestComplete = false;

            if (BL_Boost)
            {
                foreach (GameObject produce in reward)
                {
                    if (produce != null) produce.SetActive(true);
                }

                Attribution.Attributes Type = belongsTo.GetComponent<Attribution>().myAttribute;
                GameManager.instance.PowerBoost(Type, motivationAmount);

                DayCycle.instance.actionPointsUsed += actionPointWeight;
                BL_Boost = false;
            }

            if (Repeatable)
            {
                QuestFinish = false;
                BL_Boost = false;
            }
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
