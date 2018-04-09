using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class NPCInteraction : MonoBehaviour
{
    private Camera cam;
    [Header("Viewport Interaction Axis Points")]
    public float Xmin = 0.35f;
    public float Xmax = 0.65f;

    public float Ymin = 0.35f;
    public float Ymax = 0.65f;

    [Header("NPC States")]
    public bool BL_inCombat;                    //Am I in combat?
    public bool BL_HasQuest;                    //Do I have a quest?
    public bool BL_QuestCompleted = false;
    private bool BL_QuestAccepted = false;      //Did I accept a quest?

    public bool BL_InConversation = false;
    public bool BL_InTalkingRange = false;
    public static bool BL_monoTalk = false;

    public string[] flavourText;

    //----- COMPONENTS ----------------------------------------------------------
    private TaskManager TM;
    private myQuests Quests;
    private Idle myIdle;
    public Task ActiveTask;

    void Start()
    {
        TM = TaskManager.instance;
        Quests = GetComponent<myQuests>();
        myIdle = GetComponentInParent<Idle>();
        cam = Camera.main;

        if (Xmin == 0) Xmin = 0.35f;
        if (Xmax == 0) Xmax = 0.65f;
        if (Ymin == 0) Ymin = 0.35f;
        if (Ymax == 0) Ymax = 0.65f;

        TakeQuestByID();
    }

    void Update()
    {
        if (GameManager.instance.PixelMode) return;

        //If the hero's here, call return;
        if (BL_inCombat == true)
        {
            BL_HasQuest = false;
            return;
        }

        myIdle.BL_pauseMovement = WithinSpace();
        if(WithinSpace()) TalkingState();

        //Quest related
        TakeQuestByID();
        UpdateQuestFlags();
    }

    void UpdateQuestFlags()
    {
        if (BL_QuestAccepted)
            if (!ActiveTask.Quest_Complete) ActiveTask.BL_isAccepted = true;
            else
                ActiveTask.BL_isAccepted = false;
    }

    void TakeQuestByID()
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

    void TalkingState()
    {
        if (Input.GetKeyDown(KeyCode.E) && BL_InConversation == false && BL_InTalkingRange)
            Converse();

        if (BL_HasQuest && FungusDirector.instance.bl_AcceptedTask && BL_InConversation == true)
        {
            BL_QuestAccepted = true;
        }

        if (FungusDirector.instance.bl_conversationEnd)
        {
            if (BL_HasQuest && (ActiveTask.Quest_Complete || ActiveTask.Quest_Fail))
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
            string conversation = ActiveTask.ST_finishDialogue +
                FungusDirector.instance.parseKeyword + 
                ActiveTask.ST_waitingDialogue +
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

    bool WithinSpace()
    {
        Vector3 OnScreenPos = cam.WorldToViewportPoint(gameObject.transform.position);
        float x = OnScreenPos.x;
        float y = OnScreenPos.y;
        float z = OnScreenPos.z;

        if (z > 0 && x > Xmin && x < Xmax && y > Ymin && y < Ymax)
            return true;
        else
            return false;
    }

    private void QuestCompleted()
    {
        ActiveTask.Quest_Finish = true;
        ActiveTask.BL_isAccepted = false;
        ActiveTask.BL_isObtainable = false;
        ActiveTask.Quest_Complete = false;
        ActiveTask.inActiveList = false;
        BL_QuestCompleted = false;
        BL_QuestAccepted = false;
        BL_HasQuest = false;
    }
}
