using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{

    //Used by controller
    public bool BL_QuestCompleted = false;

    public bool BL_inCombat;            //Am I in combat?
    bool BL_HasQuest;                   //Do I have a quest?
    bool BL_QuestAccepted;              //Did I accept a quest?
    public int IN_NPCQuestID = 0;       //Probably not needed
    private bool BL_WithinSpace = false;//Am I inside the trigger box?

    public string[] flavourText;

    //----- INTERACTION GOs -----------------------------------------------------
    public GameObject exclaimationPoint;
    public GameObject questionMark;
    public GameObject interactionObject;

    //----- COMPONENTS ----------------------------------------------------------
    private TaskManager TM;
    private myQuests Quests;
    private Monster_Dialogue CC_Dialogue;
    public Task ActiveTask;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            Entity e_coll = other.gameObject.GetComponent<Entity>();
            if (e_coll.EntityType == Entity.Entities.Player)
                BL_WithinSpace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            Entity e_coll = other.gameObject.GetComponent<Entity>();
            if (e_coll.EntityType == Entity.Entities.Player)
            {
                Monster_Dialogue.BL_ShowDialogue = false;
                BL_WithinSpace = false;
            }
        }
    }

    void Start()
    {
        TM = TaskManager.instance;
        Quests = GetComponent<myQuests>();
        CC_Dialogue = GetComponent<Monster_Dialogue>();
    }

    void Update()
    {
        //If I'm in combat, don't bother doing things anymore
        if (BL_inCombat == true)
        {
            HideAll();
            BL_HasQuest = false;
            return;
        }

        //Check all of my quests, if any of them are obtainable, then I have a quest
        foreach (Task quest in Quests.Tasks)
        {
            if (quest == null)
                break;

            if (quest.isObtainable)
            {
                BL_HasQuest = true;
                ActiveTask = quest;
                break;
            }
        }

        UIState();

        if(BL_WithinSpace)
            if (Input.GetKeyDown(KeyCode.E))
                Converse();

        if (BL_QuestAccepted)
            if (!ActiveTask.QuestComplete) ActiveTask.isAccepted = true;
        else
            ActiveTask.isAccepted = false;
    }

    void UIState()
    {
        if (!BL_WithinSpace)
        {
            if (BL_QuestAccepted)
            {
                if (!ActiveTask.QuestComplete) AcceptedQuest();
                else HasQuest();
            }
            else
            {
                if (BL_HasQuest) HasQuest();
                else HideAll();
            }
        }else
            ShowInteraction();
    }

    void Converse()
    {
        Monster_Dialogue.BL_ShowDialogue = true;
        if(BL_QuestAccepted)
        {
            if(!ActiveTask.QuestComplete) CC_Dialogue.SetText(ActiveTask.waitingDialogue);
            else if (ActiveTask.QuestComplete)
            {
                CC_Dialogue.SetText(ActiveTask.finishDialogue);
                QuestCompleted();
            }
        }
        else if (BL_HasQuest)
        {
            CC_Dialogue.SetText(ActiveTask.descriptionDialogue);
            if (Input.GetKeyDown(KeyCode.E))
            {
                CC_Dialogue.SetText(ActiveTask.acceptedDialogue);
                BL_QuestAccepted = true;
                HideAll();
                if (Input.GetKeyDown(KeyCode.E))
                    HideAll();
            }
        }
        else
        {
            int rand = Random.Range(0, flavourText.Length - 1);

            CC_Dialogue.SetText(flavourText[rand]);
        }
    }

    #region Interaction States

    private void ShowInteraction()
    {
        exclaimationPoint.SetActive(false);
        questionMark.SetActive(false);

        interactionObject.SetActive(true);
    }

    private void HideInteraction()
    {
        exclaimationPoint.SetActive(true);
        questionMark.SetActive(false);

        interactionObject.SetActive(false);
    }

    private void AcceptedQuest()
    {
        exclaimationPoint.SetActive(false);
        questionMark.SetActive(true);

        interactionObject.SetActive(false);
    }

    private void HasQuest()
    {
        exclaimationPoint.SetActive(true);
        questionMark.SetActive(false);

        interactionObject.SetActive(false);
    }

    //Hide all
    private void HideAll()
    {
        exclaimationPoint.SetActive(false);
        interactionObject.SetActive(false);
        questionMark.SetActive(false);
    }

    #endregion

    private void QuestAccepted()
    {

    }

    private void QuestCompleted()
    {
        HideAll();
        ActiveTask.QuestFinish = true;
        BL_QuestAccepted = false;
        BL_QuestCompleted = true;
        BL_HasQuest = false;
        Debug.Log("Quest Completed");
    }
}
