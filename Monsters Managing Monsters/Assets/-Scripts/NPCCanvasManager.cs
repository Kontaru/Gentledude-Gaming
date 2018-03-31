using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCInteraction))]
public class NPCCanvasManager : MonoBehaviour {

    private NPCInteraction NPC;
    //----- INTERACTION GOs -----------------------------------------------------
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject exclaimationPoint;
    [SerializeField] private GameObject interactionObject;
    [SerializeField] private GameObject questionMark;

    void Start()
    {
        NPC = GetComponent<NPCInteraction>();
        cam = Camera.main;
        exclaimationPoint = transform.GetChild(0).gameObject;
        interactionObject = transform.GetChild(1).gameObject;
        questionMark = transform.GetChild(2).gameObject;
    }

    void Update()
    {
        if (NPC.BL_inCombat)
        {
            HideAll();
        }

        OverheadNotificationToggle();
    }

    void OverheadNotificationToggle()
    {

        Vector3 OnScreenPos = cam.WorldToViewportPoint(gameObject.transform.position);
        float x = OnScreenPos.x;
        float y = OnScreenPos.y;
        float z = OnScreenPos.z;

        if (z > 0 && x > NPC.Xmin && x < NPC.Xmax && y > NPC.Ymin && y < NPC.Ymax)
        {
            ShowInteraction();
        }
        else if (z > 0 && x > 0.05 && x < 0.95 && y > 0.05 && y < 0.95)
        {
            if (!NPC.BL_HasQuest)
            {
                HideAll();
            }
            else if (NPC.ActiveTask.BL_isAccepted && !NPC.ActiveTask.Quest_Complete)
            {
                ShowQuestionMark();
            }
            else if (!NPC.ActiveTask.BL_isAccepted || NPC.BL_QuestCompleted)
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
