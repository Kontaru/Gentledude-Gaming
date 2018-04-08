using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FungusDirector : MonoBehaviour {

    public static FungusDirector instance;
    public Flowchart FungusFlow;

    [Header("States")]
    public bool bl_Talking = false;
    public bool bl_conversationEnd = false;
    public bool bl_AcceptedTask = false;

    [Header("Dialogue")]
    public string parseKeyword;
    public string[] paragraphs;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        bl_conversationEnd = FungusFlow.GetBooleanVariable("bl_textCycleOver");
        bl_AcceptedTask = FungusFlow.GetBooleanVariable("bl_accepted");

        CheckConversationLoop();
    }

    public void CheckConversationLoop()
    {
        if (FungusFlow.GetBooleanVariable("bl_textCycleOver"))
        {
            bl_Talking = false;
            PC_Move.BL_canMove = true;
        }
    }

    public void QuestNPCDialogue(string BlockType, string dialogue, bool completed, bool failed)
    {
        paragraphs = dialogue.Split(new string[] { parseKeyword }, System.StringSplitOptions.None);
        if(BlockType == "QuestAccepted")
        {
            //Have we completed it yet?
            if (!completed)
            {
                ParagraphedConversation(paragraphs, 2);
            }
            //If we have, then what was our victory state
            else if (completed && !failed)
            {
                FungusFlow.SetStringVariable("paragraph1", paragraphs[paragraphs.Length - 2]);
            }
            else if (completed && failed)
            {
                FungusFlow.SetStringVariable("paragraph1", paragraphs[paragraphs.Length - 1]);
            }
        }
        else if (BlockType == "HasQuest")
        {
            ParagraphedConversation(paragraphs, 2);
            FungusFlow.SetStringVariable("accepted", paragraphs[paragraphs.Length - 2]);
            FungusFlow.SetStringVariable("declined", paragraphs[paragraphs.Length - 1]);
        }

        InitiateFlow(BlockType);
    }

    public void IdleNPC(string dialogue)
    {
        paragraphs = dialogue.Split(new string[] { parseKeyword }, System.StringSplitOptions.None);
        ParagraphedConversation(paragraphs, 0);
        InitiateFlow("Idle");
    }

    void ParagraphedConversation(string[] conversation, int numOptions)
    {
        for(int curParagraph = 0; curParagraph < conversation.Length - numOptions; curParagraph++)
        {
            string StringVariable = "paragraph" + (curParagraph + 1).ToString();
            FungusFlow.SetStringVariable(StringVariable, conversation[curParagraph]);
        }
    }

    void InitiateFlow(string BlockType)
    {
        PC_Move.BL_canMove = false;

        FungusFlow.SetStringVariable("BlockType", BlockType);
        Fungus.Flowchart.BroadcastFungusMessage("Talk");
    }
}
