using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_IS_Fetch : QuestPart {

    public InteractableObject[] items;
    public bool BL_QuestComplete;
	
	// Update is called once per frame
	override public void Update () {

        if (BL_IsInteractable)
        {
            foreach (InteractableObject obj in items)
            {
                if (obj.target != null)
                    obj.target.SetActive(true);
            }
        }else
        {
            foreach (InteractableObject obj in items)
            {
                if (obj.target != null)
                    obj.target.SetActive(false);
            }
        }

        CheckEndCondition();

        BL_QuestComplete = true;

        foreach (InteractableObject obj in items)
        {
            obj.CheckInteractState(GameManager.instance.Player);

            if (obj.acquired)
                Destroy(obj.target);

            if (obj.target != null)
                BL_QuestComplete = false;

        }

        if (BL_QuestComplete)
            BL_MinigameComplete = true;
	}
}
