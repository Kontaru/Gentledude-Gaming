using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Controller : Attribution
{

    //The hero unit's target
    GameObject Target;

    // -------- HERO COMPONENTS ------------ ------------ ------------ ------------ ------------
    Hero_Movement CC_Movement;
    Hero_Attack CC_Attack;

    // Use this for initialization
    void Start()
    {
        AddHero();
        Initialise();
    }

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
        //If I don't have an enemy, find one
        if (Target == null)
        {
            Target = NearestEnemy();
            CC_Movement.Target = Target;
            CC_Attack.Target = Target;
        }

        //If I still don't have an enemy, that means all enemies are dead
        if (Target == null)
        {
            Debug.Log("All targets down");
            //You lose
            return;
        }

        if (CC_Movement.BL_Alerted)
        {
            transform.LookAt(Target.transform.position);
            CC_Attack.BL_Target = true;
        }
    }

    private void LateUpdate()
    {
        TargetHandler.heroesSpawned = true;
    }

    //Initialise
    void Initialise()
    {
        //Initialise
        CC_Movement = GetComponent<Hero_Movement>();
        CC_Attack = GetComponent<Hero_Attack>();

        //Find a target and tell our movement component who it is
        Target = NearestEnemy();
        CC_Movement.Target = Target;
        CC_Attack.Target = Target;
    }

    //Adds hero to the hitlist
    void AddHero()
    {
        //Add our hero to our count, then immediately reinitialise the list
        TargetHandler.instance.heroCount++;
        TargetHandler.instance.heroes = new GameObject[TargetHandler.instance.heroCount];

        //Add myself to the hitlist for this scene
        StartCoroutine(HitList());
    }

    //Used by AddHero() - adds hero to potential targets
    IEnumerator HitList()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < TargetHandler.instance.heroes.Length; i++)
        {
            if (TargetHandler.instance.heroes[i] == null)
            {
                TargetHandler.instance.heroes[i] = gameObject;
                yield break;
            }
        }
    }

    //Target finder
    GameObject NearestEnemy()
    {
        float nearestEnemy = 100;
        GameObject currentTarget = null;

        foreach (GameObject enemy in TargetHandler.instance.monsters)
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
