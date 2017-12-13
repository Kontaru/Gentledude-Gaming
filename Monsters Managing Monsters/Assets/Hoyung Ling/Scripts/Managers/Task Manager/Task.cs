using UnityEngine;

[System.Serializable]
public class Step
{
    public string name;                     //This is a name which will display in our quest log. "Find Diana's bracelet!"
    [TextArea(2, 10)]
    public string description;              //Description of my step (how to complete it, what I need for it to be complete)
    public QuestPart requires;
    public bool complete = false;           //Is this step complete?
}

[System.Serializable]
public class Task
{
    [Header("Details")]
    public string name;                     //The name of our task. This should be something easy to distinguish. "Jeff's Quest 1" or "Diana's Bracelet".

    [TextArea(2, 10)]
    public string description;              //A description of the quest. Please fill this in so everyone else knows what the quest is and what the steps are to completing a quest.

    public int QuestID;

    public bool QuestComplete = false;     //Is our quest complete?

    public GameObject belongsTo;
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
