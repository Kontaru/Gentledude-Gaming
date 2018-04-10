using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class PDATutorial : QuestPart {

    private Camera cam;
    [Header("Viewport Interaction Axis Points")]
    public float Xmin = 0.35f;
    public float Xmax = 0.65f;

    public float Ymin = 0.35f;
    public float Ymax = 0.65f;

    public GameObject PDABoxes;

    private GameObject exclaimationPoint;
    private GameObject interactionObject;
    private GameObject questionMark;
    private bool BL_inConversation;
    private bool Quest_Complete = false;
    private bool BL_hasAccepted = false;
    private bool BL_begunTutorial = false;

    private void Start()
    {
        cam = Camera.main;
        exclaimationPoint = transform.GetChild(0).gameObject;
        interactionObject = transform.GetChild(1).gameObject;
        questionMark = transform.GetChild(2).gameObject;
    }

    override public void Update()
    {
        base.Update();

        OverheadNotificationToggle();

        if (Interactable() && Input.GetKeyDown(GameManager.instance.KC_Interact))
        {
            if(BL_IsInteractable && !BL_begunTutorial)
            {
                BL_begunTutorial = true;
                BL_hasAccepted = true;
                StartCoroutine(GettingThePDA());
            }
        }
    }

    #region PDA Tutorial

    IEnumerator GettingThePDA()
    {
        do
        {
            if(Interactable() && Input.GetKeyDown(GameManager.instance.KC_Interact))
            {
                Fungus.Flowchart.BroadcastFungusMessage("PDATutorial");
                CameraFollow.instance.otherLook = gameObject;
                BL_inConversation = true;
            }

            if(FungusDirector.instance.FungusFlow.GetBooleanVariable("bl_textCycleOver") && BL_inConversation)
            {
                BL_inConversation = false;
                CameraFollow.instance.otherLook = PDABoxes;
                yield return new WaitForSeconds(1.0f);
                CameraFollow.instance.otherLook = null;
            }
            yield return null;
        } while (PDAHandler.instance.BL_hasPDA == false);

        Quest_Complete = true;
        StartCoroutine(ViewTaskTutorial());
    }

    IEnumerator ViewTaskTutorial()
    {
        do
        {
            if (Interactable() && Input.GetKeyDown(GameManager.instance.KC_Interact))
            {
                Fungus.Flowchart.BroadcastFungusMessage("ViewTaskTutorial");
                CameraFollow.instance.otherLook = gameObject;
                BL_inConversation = true;
            }

            if (FungusDirector.instance.FungusFlow.GetBooleanVariable("bl_textCycleOver") && BL_inConversation)
            {
                CameraFollow.instance.otherLook = null;
                BL_inConversation = false;
                yield return new WaitForSeconds(1.0f);
            }
            yield return null;
        } while (!BL_inConversation == false && !Input.GetKeyDown(KeyCode.N));

        Quest_Complete = true;
    }

    IEnumerator PingingQuests()
    {
        do
        {
            if (Interactable() && Input.GetKeyDown(GameManager.instance.KC_Interact))
            {
                Fungus.Flowchart.BroadcastFungusMessage("LocatorTutorial");
                CameraFollow.instance.otherLook = gameObject;
                BL_inConversation = true;
            }

            if (FungusDirector.instance.FungusFlow.GetBooleanVariable("bl_textCycleOver") && BL_inConversation)
            {
                CameraFollow.instance.otherLook = null;
                BL_inConversation = false;
                yield return new WaitForSeconds(1.0f);
            }
            yield return null;
        } while (!BL_inConversation == false && CameraFollow.instance.otherLook == CurrentTasks.instance.currentTask[0].GO_belongsTo);

        Quest_Complete = true;
    }

    #endregion

    bool Interactable()
    {

        Vector3 OnScreenPos = cam.WorldToViewportPoint(gameObject.transform.position);

        float x = OnScreenPos.x;
        float y = OnScreenPos.y;
        float z = OnScreenPos.z;

        if (z > 0 && x > Xmin && x < Xmax && y > Ymin && y < Ymax)
        {
            return true;
        }
        else
            return false;
    }

    void OverheadNotificationToggle()
    {

        Vector3 OnScreenPos = cam.WorldToViewportPoint(gameObject.transform.position);
        float x = OnScreenPos.x;
        float y = OnScreenPos.y;
        float z = OnScreenPos.z;

        if (z > 0 && x > Xmin && x < Xmax && y > Ymin && y < Ymax)
        {
            ShowInteraction();
        }
        else if (z > 0 && x > 0.05 && x < 0.95 && y > 0.05 && y < 0.95)
        {
            if (!BL_IsInteractable)
            {
                HideAll();
            }
            else if (BL_hasAccepted && !Quest_Complete)
            {
                ShowQuestionMark();
            }
            else
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
