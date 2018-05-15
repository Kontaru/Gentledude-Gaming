using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class SimpleQuestPart : QuestPart {

    public GameObject objectiveMarker;
    public GameObject[] VisibilityTargets;
    [TextArea(2, 10)]
    public string dialogueOnContact;
    public string acceptDialogue;
    public string declineDialogue;

    private Flowchart flowchart;
    bool BL_ObjectiveMarkersOn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!BL_IsInteractable) return;

        if (other.gameObject.GetComponent<Entity>() != null)
        {
            Entity e_coll = other.gameObject.GetComponent<Entity>();
            if (e_coll.EntityType == Entity.Entities.Player)
            {
                if (objectiveMarker != null)
                    objectiveMarker.SetActive(false);

                if(dialogueOnContact != "")
                {
                    flowchart.SetStringVariable("dialogue", dialogueOnContact);
                    flowchart.SetStringVariable("acceptText", acceptDialogue);
                    flowchart.SetStringVariable("declineText", declineDialogue);
                    Flowchart.BroadcastFungusMessage("InitiateStep");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!BL_IsInteractable) return;

        if (other.gameObject.GetComponent<Entity>() != null)
        {
            Entity e_coll = other.gameObject.GetComponent<Entity>();
            if (e_coll.EntityType == Entity.Entities.Player)
            {
                if (objectiveMarker != null)
                    objectiveMarker.SetActive(true);
            }
        }
    }

    private void Start()
    {
        flowchart = MinorFlowchartInstance.instance;
    }

    public override void Update()
    {
        base.Update();

        if(BL_IsInteractable)
        {
            foreach (GameObject target in VisibilityTargets)
            {
                target.SetActive(true);
            }
            if (!BL_ObjectiveMarkersOn)
            {
                if (objectiveMarker != null)
                    objectiveMarker.SetActive(true);
                BL_ObjectiveMarkersOn = true;
            }
        }else
        {
            foreach (GameObject target in VisibilityTargets)
            {
                target.SetActive(false);
            }
        }


        if (flowchart.GetBooleanVariable("bl_textCycleOver") == true)
        {
            if (BL_IsInteractable)
            {
                BL_MinigameComplete = flowchart.GetBooleanVariable("bl_accepted");
                BL_ObjectiveMarkersOn = false;
            }
            Invoke("Reset", 1.0f);
        }

        if (!BL_IsInteractable) return;
        CheckEndCondition();
    }

    private void Reset()
    {
        flowchart.SetBooleanVariable("bl_accepted", false);
        flowchart.SetBooleanVariable("bl_textCycleOver", false);
    }
}
