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
    public string TaskBrief;
    [TextArea(2, 10)]
    public string DescriptionDialogue;              //A description of the quest. Please fill this in so everyone else knows what the quest is and what the steps are to completing a quest.
    [TextArea(2, 10)]
    public string AcceptedDialogue;
    [TextArea(2, 10)]
    public string DeclinedDialogue;

    [Header("Finishing a quest")]
    [TextArea(2, 10)]
    public string FinishDialogue;
    [TextArea(2, 10)]
    public string FailDialogue;
    [TextArea(2, 10)]
    public string WaitingDialogue;


    public int Quest_ID;
    public bool Repeatable = false;

    public enum Values
    {
        None,
        Level_01,
        Level_02,
        Level_03
    }

    [Header("Action Points + Motivation")]
    public Values Presets;
    public int customActionPointWeight;
    public int customMotivationAmount;

    //[Header("Steps")]
    //public Step[] Steps;                    //Our steps to completing the quest
}
