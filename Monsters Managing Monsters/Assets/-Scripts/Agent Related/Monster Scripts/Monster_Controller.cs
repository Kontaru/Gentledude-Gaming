using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Controller : Attribution {

    // -------- MONSTER COMPONENTS ------------ ------------ ------------ ------------ ------------
    NPCInteraction CC_Interaction;
    Idle CC_Idle;

    void Start()
    {
        CC_Interaction = transform.GetChild(0).GetComponent<NPCInteraction>();
        CC_Idle = GetComponent<Idle>();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        //Combat begins if heroes are in the scene
        //Otherwise, do normal stuff
        if(CC_Interaction.BL_inCombat == true) return;
        CC_Idle.BL_isIdle = !CC_Interaction.BL_HasQuest;

        if(CC_Interaction.BL_QuestCompleted)
        {
            CC_Interaction.BL_QuestCompleted = false;
        }
    }
}
