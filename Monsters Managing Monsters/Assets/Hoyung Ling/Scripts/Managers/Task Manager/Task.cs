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

    [TextArea(2, 10)]
    public string descriptionDialogue;              //A description of the quest. Please fill this in so everyone else knows what the quest is and what the steps are to completing a quest.
    [TextArea(2, 10)]
    public string acceptedDialogue;
    [TextArea(2, 10)]
    public string finishDialogue;
    [TextArea(2, 10)]
    public string waitingDialogue;

    public int QuestID;

    public bool QuestComplete = false;     //Is our quest complete?
    public bool QuestFinish = false;       //Trigger for NPC hand in

    public GameObject belongsTo;
    public GameObject[] reward;
    public bool isObtainable = false;
    public bool isAccepted = false;

    [Header("Steps")]
    public Step[] Steps;                    //Our steps to completing the quest
    //Checks if all our steps are complete
    public void StepChecker()
    {
        //For each step
        foreach (Step step in Steps)
        {
            //If any of these steps are false, just stop everything and quit the function.
            if (step.complete == false)
            {
                if (isAccepted)
                {
                    if (step.requires.GetComponent<QuestPart>().BL_MinigameComplete)
                        step.complete = true;

                    step.requires.GetComponent<QuestPart>().BL_IsInteractable = true;
                }
                return;
            }
        }
        //If return is never called, then we can safetly set this to true.

        QuestComplete = true;

        if (QuestFinish)
        {
            foreach (GameObject produce in reward)
            {
                produce.SetActive(true);
            }
        }
    }

    public void Obtained()
    {
        if(isAccepted)
        {
            foreach (Step step in Steps)
            {
                //If any of these steps are false, just stop everything and quit the function.
                if (step.complete == false)
                {

                }
            }
        }
    }
}
