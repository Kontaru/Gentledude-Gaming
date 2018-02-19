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
    [SerializeField] private bool BL_QuestAccepted = false;      //Did I accept a quest?
    [SerializeField] private bool BL_WithinSpace = false;        //Am I inside the trigger box?

    public bool BL_InConversation = false;

    public string[] flavourText;

    //----- INTERACTION GOs -----------------------------------------------------
    public GameObject exclaimationPoint;
    public GameObject questionMark;
    public GameObject interactionObject;
    public ScrollRect textScrollRect;

    //----- COMPONENTS ----------------------------------------------------------
    private TaskManager TM;
    private myQuests Quests;
    private Idle myIdle;
    public static Flowchart CC_Dialogue;
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
        textScrollRect = GameObject.Find("SayWindow").GetComponent<ScrollRect>();
    }

    void Update()
    {
        if (GameManager.instance.PixelMode) return;

        //If I'm in combat, don't bother doing things anymore
        if (BL_inCombat == true)
        {
            HideAll();
            BL_HasQuest = false;
            return;
        }

        QuestChecker();

        UIState();
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

    void UIState()
    {
        if (!BL_WithinSpace)
        {
            if (BL_QuestAccepted)
            {
                if (!ActiveTask.Quest_Complete && !ActiveTask.Quest_Fail) AcceptedQuest();
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

    void ConversationChecker()
    {
        if (BL_WithinSpace)
            if (Input.GetKeyDown(KeyCode.E) && BL_InConversation == false)
                Converse();

        if (CC_Dialogue.GetBooleanVariable("bl_accepted") && BL_InConversation == true)
        {
            BL_QuestAccepted = true;
            CC_Dialogue.SetBooleanVariable("bl_accepted", false);
        }

        if (CC_Dialogue.GetBooleanVariable("bl_textCycleOver"))
        {
            if (ActiveTask.Quest_Complete || ActiveTask.Quest_Fail)
            {
                QuestCompleted();
            }

            BL_InConversation = false;
            Invoke("ConvoOff", 1.0f);
            PC_Move.BL_canMove = true;

        }
    }

    void ConvoOff()
    {
        CC_Dialogue.SetBooleanVariable("bl_textCycleOver", false);
    }

    void Converse()
    {
        PC_Move.BL_canMove = false;
        BL_InConversation = true;
        textScrollRect.verticalNormalizedPosition = 1f;

        if (BL_QuestAccepted)
        {
            if (!ActiveTask.Quest_Complete )
            {
                CC_Dialogue.SetStringVariable("QuestStatus", ActiveTask.ST_waitingDialogue);
            }
            else if (ActiveTask.Quest_Complete && !ActiveTask.Quest_Fail)
            {
                CC_Dialogue.SetStringVariable("QuestStatus", ActiveTask.ST_finishDialogue);
            }
            else if (ActiveTask.Quest_Complete && ActiveTask.Quest_Fail)
            {
                CC_Dialogue.SetStringVariable("QuestStatus", ActiveTask.ST_failDialogue);
            }

            Fungus.Flowchart.BroadcastFungusMessage("QuestAccepted");
        }
        else if (BL_HasQuest)
        {
            CC_Dialogue.SetStringVariable("description", ActiveTask.ST_descriptionDialogue);
            CC_Dialogue.SetStringVariable("accepted", ActiveTask.ST_acceptedDialogue);
            CC_Dialogue.SetStringVariable("declined", ActiveTask.ST_declinedDialogue);

            Fungus.Flowchart.BroadcastFungusMessage("HasQuest");
        }
        else
        {
            int rand = Random.Range(0, flavourText.Length - 1);
            CC_Dialogue.SetStringVariable("Idle", flavourText[rand]);

            Fungus.Flowchart.BroadcastFungusMessage("Idle");
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

    private void QuestCompleted()
    {
        HideAll();
        ActiveTask.Quest_Finish = true;
        BL_QuestCompleted = false;
        BL_QuestAccepted = false;
        BL_HasQuest = false;      
    }
}
