using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_IS_Distribution : QuestPart {

    public InteractableObject[] items;
    public bool BL_QuestComplete;

    public int IN_maxItemCount;
    int IN_itemCount;
    [HideInInspector]
    public int IN_stolenCount = 0;

    [SerializableField]
    int IN_FinalCount;

    bool BL_FirstLoad = true;

    // Update is called once per frame
    override public void Update()
    {
        SetObjectStates();

        if (!BL_IsInteractable) return;

        CheckFailState();
        CheckWinState();

        if (BL_QuestComplete)
        {
            BL_MinigameComplete = true;
            BL_FirstLoad = true;
        }

        CheckEndCondition();
    }

    void SetObjectStates()
    {
        if (BL_IsInteractable)
        {
            if (BL_FirstLoad)
            {
                IN_FinalCount = IN_maxItemCount;
                IN_itemCount = IN_maxItemCount;
                foreach (InteractableObject obj in items)
                {
                    if (obj.target != null)
                    {
                        obj.target.SetActive(true);
                        obj.SetCanvasElements();
                    }
                }
                BL_FirstLoad = false;
            }
        }
        else
        {
            foreach (InteractableObject obj in items)
            {
                if (obj.target != null)
                    obj.target.SetActive(false);
            }
        }
    }

    void CheckFailState()
    {
        if (IN_FinalCount < items.Length)
        {
            BL_MinigameComplete = true;
            BL_MinigameFail = true;
            BL_FirstLoad = true;
            return;
        }
    }

    void CheckWinState()
    {
        BL_QuestComplete = true;
        int itemCount = 0;

        foreach (InteractableObject obj in items)
        {
            obj.CheckInteractState(GameManager.instance.Player);

            if (obj.acquired == true)
            {
                obj.target.SetActive(false);
                itemCount++;
            }

            if (obj.acquired == false)
                BL_QuestComplete = false;

        }

        IN_FinalCount = IN_itemCount - IN_stolenCount - itemCount;
    }
}
