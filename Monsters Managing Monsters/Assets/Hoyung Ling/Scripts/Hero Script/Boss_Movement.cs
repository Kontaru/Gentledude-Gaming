using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Boss_Movement : MonoBehaviour
{

    [Header("Direction")]
    public Transform Left;
    public Transform Right;
    public Transform Behind;

    //Dashes
    [Header("Dash Variables")]
    public bool BL_Dash = false;
    public float FL_moveSpeed;
    float FL_defaultSpeed;

    [Header("Dash Cooldown")]
    float FL_Cooldown;
    public float FL_Delay = 5.0f;

    public float FL_DashFor = 2;
    float FL_DashTimer = 0;

    //Important variables
    NavMeshAgent enemy;

    public static GameObject Target;
    public bool BL_Alerted = false;
    public bool BL_Move = false;
    public float moveSpeed;

    // Use this for initialization
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        FL_Cooldown = Time.time;
        FL_defaultSpeed = FL_moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!BL_Alerted && Vector3.Distance(Target.transform.position, transform.position) < 3.0f)
        {
            AudioManager.instance.Stop("Dungeon Music");
            AudioManager.instance.Play("Boss Music");
            FL_Cooldown = Time.time + FL_Delay;
            BL_Alerted = true;
            BL_Move = true;
        }

        if (BL_Alerted && BL_Move)
        {
            if (!BL_Dash && Time.time > FL_Cooldown)
            {
                FL_DashTimer = Time.time + FL_DashFor;
                BL_Dash = true;
            }

            if (BL_Dash)
            {
                DashSideways();
            }
            else
            {
                if (Vector3.Distance(Target.transform.position, transform.position) < 6.0f)
                    RunFrom();
                else if (Vector3.Distance(Target.transform.position, transform.position) >= 6.0f)
                    ChaseTo();
            }
        }
    }

    public void ChaseTo()
    {
        enemy.destination = Target.transform.position;
        enemy.speed = moveSpeed;
    }

    public void RunFrom()
    {
        enemy.destination = Behind.position;
        enemy.speed = moveSpeed * 4.0f;
    }

    public void DashSideways()
    {
        int rand = Random.Range(0, 1);

        if (rand == 0)
        {
            enemy.destination = Left.position;
            enemy.speed = moveSpeed * 20.0f;
        }
        else if (rand == 1)
        {
            enemy.destination = Right.position;
            enemy.speed = moveSpeed * 20.0f;
        }

        if (Time.time > FL_DashTimer)
        {
            FL_Cooldown = Time.time + FL_Delay;
            BL_Dash = false;
        }
    }
}
