using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_OS_Minigame_Handler : QuestPart {

    public int minigameIndex;
    public PDAHandler PDA;
    public bool BL_Activatable = true;
    private bool BL_finished = false;

	// Use this for initialization
	void Start () {
        PDA = PDAHandler.instance;
	}
	
	// Update is called once per frame
	override public void Update () {
        base.Update();

        if (!BL_IsInteractable) return;

        if (BL_Activatable)
        {
            PDA.StartMinigame(minigameIndex, true);
            BL_Activatable = false;
        }

        if (BL_MinigameComplete)
        {
            if (!BL_finished)
            {
                PDA.OnClickClose();
                BL_Activatable = true;
                BL_finished = true;
            }            
        }

        CheckEndCondition();
	}
}
