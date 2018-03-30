using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCInteraction))]
public class NPCCanvasManager : MonoBehaviour {

    private NPCInteraction NPC;
    //----- INTERACTION GOs -----------------------------------------------------
    public GameObject exclaimationPoint;
    public GameObject questionMark;
    public GameObject interactionObject;

    void Start()
    {
        InvokeRepeating("OverheadNotificationToggle", 0, 1);
    }

    void Update()
    {
        if (NPC.BL_inCombat)
        {
            HideAll();
        }
    }
    void OverheadNotificationToggle()
    {

        Vector3 OnScreenPos = Camera.main.ScreenToViewportPoint(gameObject.transform.position);

        float x = OnScreenPos.x;
        float y = OnScreenPos.y;
        float z = OnScreenPos.z;

        if (z > 0 && x > 0.4 && x < 0.6 && y > 0.4 && y < 0.4)
        {
            ShowInteraction();
        }
        else if (z > 0 && x > 0 && x < 1 && y > 0 && y < 1)
        {
            if (!NPC.ActiveTask.Quest_Complete && !NPC.ActiveTask.Quest_Fail)
            {
                ShowQuestionMark();
            }
            else if (NPC.BL_HasQuest || NPC.BL_QuestCompleted)
            {
                ShowExclamation();
            }
        }
        else
        {
            HideAll();
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

    private void ShowQuestionMark()
    {
        exclaimationPoint.SetActive(false);
        questionMark.SetActive(true);

        interactionObject.SetActive(false);
    }

    private void ShowExclamation()
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
}
