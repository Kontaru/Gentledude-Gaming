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

    private GameObject exclaimationPoint;
    private GameObject interactionObject;
    private GameObject questionMark;

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

        if (Interactable() && Input.GetKeyDown(KeyCode.E))
        {
            Fungus.Flowchart.BroadcastFungusMessage("PDATutorial");
        }
    }

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
