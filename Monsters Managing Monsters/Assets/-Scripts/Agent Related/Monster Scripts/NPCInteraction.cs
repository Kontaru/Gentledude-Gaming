using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class NPCInteraction : MonoBehaviour
{

    //Used by controller
    public bool BL_QuestCompleted = false;

    public bool BL_inCombat;                    //Am I in combat?
    public bool BL_HasQuest;                    //Do I have a quest?
    private bool BL_QuestAccepted = false;      //Did I accept a quest?
    private bool BL_WithinSpace = false;        //Am I inside the trigger box?

    public bool BL_InConversation = false;

    public string[] flavourText;

    //----- INTERACTION GOs -----------------------------------------------------
    public GameObject exclaimationPoint;
    public GameObject questionMark;
    public GameObject interactionObject;

    //----- COMPONENTS ----------------------------------------------------------
    private TaskManager TM;
    private myQuests Quests;
    private Idle myIdle;
    public Task ActiveTask;

    #region Triggers

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            Entity e_coll = other.gameObject.GetComponent<Entity>();
            if (e_coll.EntityType == Entity.Entities.Player)
            {
                BL_WithinSpace = true;
                myIdle.BL_pauseMovement = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            Entity e_coll = other.gameObject.GetComponent<Entity>();
            if (e_coll.EntityType == Entity.Entities.Player)
            {
                BL_WithinSpace = false;
                myIdle.BL_pauseMovement = false;
            }
        }
    }

    #endregion

    void Start()
    {
        TM = TaskManager.instance;
        Quests = GetComponent<myQuests>();
        myIdle = GetComponentInParent<Idle>();

        QuestChecker();
    }

    void Update()
    {
        if (GameManager.instance.PixelMode) return;

        //If I'm in combat, don't bother doing things anymore
        if (BL_inCombat == true)
        {
            BL_HasQuest = false;
            return;
        }


        QuestChecker();

        ConversationChecker();

        UpdateFlags();
    }

    void UpdateFlags()
    {
        if (BL_QuestAccepted)
            if (!ActiveTask.Quest_Complete) ActiveTask.BL_isAccepted = true;
            else
                ActiveTask.BL_isAccepted = false;
    }

    void QuestChecker()
    {
        if (Quests != null)
        {
            //Check all of my quests, if any of them are obtainable, then I have a quest
            foreach (Task quest in Quests.Tasks)
            {
                if (quest == null)
                    break;

                quest.GO_belongsTo = transform.parent.gameObject;

                if (quest.BL_isObtainable)
                {
                    BL_HasQuest = true;
                    ActiveTask = quest;
                    break;
                }
            }
        }
        else
        {
            BL_HasQuest = false;
            BL_QuestAccepted = false;
        }
    }

    void ConversationChecker()
    {
        if (BL_WithinSpace)
            if (Input.GetKeyDown(KeyCode.E) && BL_InConversation == false)
                Converse();

        if (FungusDirector.instance.bl_AcceptedTask && BL_InConversation == true)
        {
            BL_QuestAccepted = true;
        }

        if (FungusDirector.instance.bl_conversationEnd)
        {
            if (ActiveTask.Quest_Complete || ActiveTask.Quest_Fail)
            {
                QuestCompleted();
            }

            BL_InConversation = false;
        }

        FungusDirector.instance.bl_Talking = BL_InConversation;
    }

    void Converse()
    {
        BL_InConversation = true;

        if (BL_QuestAccepted)
        {
            string conversation = ActiveTask.ST_waitingDialogue +
                FungusDirector.instance.parseKeyword +
                ActiveTask.ST_finishDialogue +
                FungusDirector.instance.parseKeyword +
                ActiveTask.ST_failDialogue;

            FungusDirector.instance.QuestNPCDialogue("QuestAccepted", 
                conversation, 
                ActiveTask.Quest_Complete, 
                ActiveTask.Quest_Fail);
        }
        else if (BL_HasQuest)
        {
            string conversation = ActiveTask.ST_descriptionDialogue +
                FungusDirector.instance.parseKeyword +
                ActiveTask.ST_acceptedDialogue +
                FungusDirector.instance.parseKeyword +
                ActiveTask.ST_declinedDialogue;

            FungusDirector.instance.QuestNPCDialogue("HasQuest",
                conversation,
                false,
                false);
        }
        else
        {
            int rand = Random.Range(0, flavourText.Length - 1);
            FungusDirector.instance.IdleNPC(flavourText[rand]);
        }
    }

    private void QuestCompleted()
    {
        ActiveTask.Quest_Finish = true;
        BL_QuestCompleted = false;
        BL_QuestAccepted = false;
        BL_HasQuest = false;      
    }
}
