using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_IS_Fetch : QuestPart {

    public InteractableObject[] items;
    public GameObject[] VisibilityTargets;
    public bool BL_QuestComplete;

    bool BL_FirstLoad = true;
	
	// Update is called once per frame
	override public void Update () {

        SetObjectStates();

        if (!BL_IsInteractable) return;

        CheckEndCondition();

        BL_QuestComplete = true;

        foreach (InteractableObject obj in items)
        {

            if (obj.acquired)
                Destroy(obj.target);

            if (obj.target != null)
                BL_QuestComplete = false;

        }

        if (BL_QuestComplete)
        {
            OpenDoor.instance.QuestEnded = true;
            BL_MinigameComplete = true;
            BL_FirstLoad = true;
        }
	}

    void SetObjectStates()
    {
        if (BL_IsInteractable)
        {
            if (BL_FirstLoad)
            {
                MakeVisible(true);
                BL_FirstLoad = false;
            }
        }
        else MakeVisible(false);
    }

    void MakeVisible(bool state)
    {
        if (state == true)
        {
            foreach (GameObject target in VisibilityTargets)
            {
                target.SetActive(true);
            }

            foreach (InteractableObject obj in items)
            {
                if (obj.target != null)
                    obj.target.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject target in VisibilityTargets)
            {
                target.SetActive(false);
            }

            foreach (InteractableObject obj in items)
            {
                if (obj.target != null)
                    obj.target.SetActive(false);
            }
        }
    }
}
