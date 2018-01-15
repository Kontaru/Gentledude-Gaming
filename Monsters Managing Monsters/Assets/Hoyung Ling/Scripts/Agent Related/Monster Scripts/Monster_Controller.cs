using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Controller : Attribution {

    //The hero unit's target
    public GameObject Target;

    // -------- MONSTER COMPONENTS ------------ ------------ ------------ ------------ ------------
    Monster_Combat CC_Combat;
    NPCInteraction CC_Interaction;
    Idle CC_Idle;

    void Start()
    {
        AddMonster();

        CC_Combat = GetComponent<Monster_Combat>();
        CC_Interaction = transform.GetChild(0).GetComponent<NPCInteraction>();
        CC_Idle = GetComponent<Idle>();

        Target = NearestEnemy();
        CC_Combat.Target = Target;
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        //Combat begins if heroes are in the scene
        //Otherwise, do normal stuff
        if (TargetHandler.instance.heroCount > 0)
        {
            CC_Combat.BL_InitiateCombat = true;
            CC_Interaction.BL_inCombat = true;
            CombatLoop();
            return;
        }
        else
        {
            CC_Combat.BL_InitiateCombat = false;
            CC_Interaction.BL_inCombat = false;
            CC_Idle.isIdle = !CC_Interaction.BL_HasQuest;

            if(CC_Interaction.BL_QuestCompleted)
            {
                CC_Interaction.BL_QuestCompleted = false;
                GameManager.instance.PowerBoost(myAttribute);
            }
        }
    }

    void CombatLoop()
    {
        //If I don't have an enemy, find one
        if (Target == null)
        {
            Target = NearestEnemy();
            CC_Combat.Target = Target;
        }

        //If I still don't have an enemy, that means all enemies are dead
        if (Target == null)
        {
            Debug.Log("All targets down");
            //You lose
            return;
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

    //Target finder
    GameObject NearestEnemy()
    {
        float nearestEnemy = 100;
        GameObject currentTarget = null;

        foreach (GameObject enemy in TargetHandler.instance.heroes)
        {
            if (enemy != null)
            {
                float comparisonEnemy = Vector3.Distance(enemy.transform.position, transform.position);

                if (comparisonEnemy < nearestEnemy)
                {
                    nearestEnemy = comparisonEnemy;
                    currentTarget = enemy;
                }
            }
        }

        return currentTarget;
    }
}
