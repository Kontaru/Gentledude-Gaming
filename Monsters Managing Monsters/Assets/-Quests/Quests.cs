using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class Step
//{
//    public string name;                     //This is a name which will display in our quest log. "Find Diana's bracelet!"
//    [TextArea(2, 10)]
//    public string description;              //Description of my step (how to complete it, what I need for it to be complete)
//    public QuestPart requires;
//    public bool TurnOff = false;
//    public bool complete = false;           //Is this step complete?
//    public bool active = false;
//}

[CreateAssetMenu (fileName = "New Quest", menuName = "Quest")]
public class Quests : ScriptableObject {

    [Header("Details")]
    public new string name;                     //The name of our task. This should be something easy to distinguish. "Jeff's Quest 1" or "Diana's Bracelet".

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
    public bool Repeatable = false;

    [Header("Action Points + Motivation")]
    public int IN_actionPointWeight;
    public int IN_motivationAmount;

    //[Header("Steps")]
    //public Step[] Steps;                    //Our steps to completing the quest
}
