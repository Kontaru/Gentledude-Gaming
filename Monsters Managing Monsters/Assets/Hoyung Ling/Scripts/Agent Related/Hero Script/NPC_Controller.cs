using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : Attribution
{

    public GameObject Enemies;

    NPC_Movement CC_Movement;
    NPC_Attack CC_Attack;

    //-----------------------------------------------------------
    public GameObject GO_Billboard;
    EnemyBillboard EB_Sprite;

    // Use this for initialization
    void Start()
    {
        EB_Sprite = GO_Billboard.GetComponent<EnemyBillboard>();

        CC_Movement = GetComponent<NPC_Movement>();
        CC_Attack = GetComponent<NPC_Attack>();

        Enemies = NearestEnemy();
        NPC_Movement.Target = Enemies;
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemies == null)
        {
            Enemies = NearestEnemy();
            NPC_Movement.Target = Enemies;
        }

        if(Enemies == null)
        {
            //You lose
        }

        if (CC_Movement.BL_Alerted)
        {
            transform.LookAt(Enemies.transform.position);
            EB_Sprite.BL_Spotted = true;
            CC_Attack.BL_Target = true;
        }
        else
            EB_Sprite.BL_Spotted = false;
    }

    GameObject NearestEnemy()
    {
        float nearestEnemy = 100;

        foreach (GameObject enemy in GameManager.instance.heroTargets)
        {
            if (enemy != null)
            {
                float comparisonEnemy = Vector3.Distance(enemy.transform.position, transform.position);

                if (comparisonEnemy < nearestEnemy)
                    return enemy;
            }
        }

        return null;
    }
}
