using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_IS_Distribution : QuestPart {

    public InteractableObject[] items;
    public bool BL_QuestComplete;

    public int IN_ItemCount;

    bool BL_FirstLoad = true;

    // Update is called once per frame
    void Update()
    {
        Fluff();
        if (BL_IsInteractable)
        {
            foreach (InteractableObject obj in items)
            {
                if (obj.target != null)
                    obj.target.SetActive(true);
            }
        }
        else
        {
            foreach (InteractableObject obj in items)
            {
                if (obj.target != null)
                    obj.target.SetActive(false);
            }
            return;
        }

        if(IN_ItemCount < items.Length)
        {
            
            return;
        }

        CheckEndCondition();

        BL_QuestComplete = true;

        foreach (InteractableObject obj in items)
        {
            obj.CheckInteractState(GameManager.instance.Player);

            if (obj.acquired)
            {
                Destroy(obj.target);
                IN_ItemCount -= 1;
            }

            if (obj.target != null)
                BL_QuestComplete = false;

        }

        if (BL_QuestComplete)
            BL_MinigameComplete = true;
    }

    virtual public void Fluff()
    {

    }
}
