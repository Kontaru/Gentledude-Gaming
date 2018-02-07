using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Controller : Attribution {

    // -------- MONSTER COMPONENTS ------------ ------------ ------------ ------------ ------------
    NPCInteraction CC_Interaction;
    Idle CC_Idle;

    void Start()
    {
        AddMonster();

        CC_Interaction = transform.GetChild(0).GetComponent<NPCInteraction>();
        CC_Idle = GetComponent<Idle>();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        //Combat begins if heroes are in the scene
        //Otherwise, do normal stuff
        if (TargetHandler.instance.heroCount > 0)
        {
            CC_Interaction.BL_inCombat = true;
            return;
        }
        else
        {
            CC_Interaction.BL_inCombat = false;
            CC_Idle.isIdle = !CC_Interaction.BL_HasQuest;

            if(CC_Interaction.BL_QuestCompleted)
            {
                CC_Interaction.BL_QuestCompleted = false;
            }
        }
    }

    //Add monster to the hitlist
    void AddMonster()
    {
        //Add our enemy to our count, then immediately reinitialise the list
        TargetHandler.instance.monsterCount = TargetHandler.instance.monsterCount + 1;
        TargetHandler.instance.monsters = new GameObject[TargetHandler.instance.monsterCount];

        //Add myself to the hitlist for this scene
        StartCoroutine(HitList());
    }

    //Used by AddMonster() - adds monster to potential targets
    IEnumerator HitList()
    {
        //Set delay so that all monsters have loaded in
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < TargetHandler.instance.monsters.Length; i++)
        {
            if (TargetHandler.instance.monsters[i] == null)
            {
                TargetHandler.instance.monsters[i] = gameObject;
                yield break;
            }
        }
    }
}
