using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{

    public bool BL_inCombat;            //Am I in combat?
    bool BL_HasQuest;                   //Do I have a quest?
    bool BL_QuestAccepted;              //Did I accept a quest?
    public int IN_NPCQuestID = 0;       //Probably not needed
    private bool BL_WithinSpace = false;//Am I inside the trigger box?

    //----- INTERACTINO GOs -----------------------------------------------------
    public GameObject exclaimationPoint;
    public GameObject interactionObject;

    //----- COMPONENTS ----------------------------------------------------------
    private Text interactionText;
    private TaskManager TM;
    private myQuests Quests;
    private Task ActiveTask;

    //When something enters the collider
    //Check if the colliding type is of type "Player"
    //If so, allow for an interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            Entity e_coll = other.gameObject.GetComponent<Entity>();
            if (e_coll.EntityType == Entity.Entities.Player && !BL_QuestAccepted)
            {
                if (BL_HasQuest) ShowInteraction();
                BL_WithinSpace = true;
            }
        }
    }

    //When something exits the collider
    //Check if the colliding type is of type "Player"
    //If so, disable the interaction
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            Entity e_coll = other.gameObject.GetComponent<Entity>();
            if (e_coll.EntityType == Entity.Entities.Player && !BL_QuestAccepted)
            {
                if (BL_HasQuest) HideInteraction();
                BL_WithinSpace = false;
            }
        }
    }

    //When something stays in the collider
    //Check if the colliding type is of type "Player"
    //If so, allow interaction of the player with the entity
    //All code related to interaction with the entity should be here
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            Entity e_coll = other.gameObject.GetComponent<Entity>();
            if (e_coll.EntityType == Entity.Entities.Player && !BL_QuestAccepted)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (ActiveTask.QuestComplete) QuestCompleted();
                    else QuestAccepted();
                }
            }
        }
    }

    void Start()
    {
        TM = TaskManager.instance;
        Quests = GetComponent<myQuests>();
    }

    void Update()
    {
        //If I'm in combat, don't bother doing things anymore
        if (BL_inCombat == true) return;

        //Check all of my quests, if any of them are obtainable, then I have a quest
        foreach (Task quest in Quests.Tasks)
        {
            if (quest.isObtainable)
            {
                BL_HasQuest = true;
                ActiveTask = quest;
                break;
            }
        }

        //If I've accepted a quest, don't bother with anything else below
        if (BL_QuestAccepted)
        {
            ActiveTask.isAccepted = true;
            return;
        }

        if (BL_HasQuest && !BL_WithinSpace) HasQuest();
        else if (!BL_HasQuest && !BL_WithinSpace) HideAll();
    }

    private void ShowInteraction()
    {
        exclaimationPoint.SetActive(false);
        interactionObject.SetActive(true);
    }

    private void HideInteraction()
    {
        interactionObject.SetActive(false);
        exclaimationPoint.SetActive(true);
    }

    private void HasQuest()
    {
        exclaimationPoint.SetActive(true);
        interactionObject.SetActive(false);
    }

    private void HideAll()
    {
        exclaimationPoint.SetActive(false);
        interactionObject.SetActive(false);
    }

    private void QuestAccepted()
    {
        BL_QuestAccepted = true;
        HideAll();
    }

    private void QuestCompleted()
    {
        Debug.Log("Quest Completed");
    }}