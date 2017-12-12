using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{

    public bool BL_HasQuest;
    public bool BL_QuestAccepted;
    public int IN_NPCQuestID = 0;

    public GameObject exclaimationPoint;
    public GameObject interactionObject;

    private Text interactionText;
    private TaskManager TM;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            if (other.gameObject.GetComponent<Entity>().EntityType == Entity.Entities.Player && !BL_QuestAccepted)
            {
                if (BL_HasQuest) ShowInteraction();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            if (other.gameObject.GetComponent<Entity>().EntityType == Entity.Entities.Player && !BL_QuestAccepted)
            {
                if (BL_HasQuest) HideInteraction();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Entity>() != null)
        {
            if (other.gameObject.GetComponent<Entity>().EntityType == Entity.Entities.Player && !BL_QuestAccepted)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (TM.Tasks[IN_NPCQuestID].QuestComplete) QuestCompleted();
                    else QuestAccepted();
                }
            }
        }
    }

    void Start()
    {

        TM = TaskManager.instance;

    }

    void Update()
    {
        if (BL_HasQuest) ShowExclaimantion();
        else HideAll();
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

    private void ShowExclaimantion()
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
        Debug.Log("Add Quest to TaskManager");
    }

    private void QuestCompleted()
    {
        Debug.Log("Quest Completed");
    }
}
