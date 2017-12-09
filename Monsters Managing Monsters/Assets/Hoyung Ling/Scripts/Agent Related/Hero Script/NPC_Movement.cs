using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class NPC_Movement : MonoBehaviour
{

    [Header("Direction")]
    public Transform Left;
    public Transform Right;
    public Transform Behind;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!BL_Alerted && Vector3.Distance(Target.transform.position, transform.position) < 10.0f)
        {
            BL_Alerted = true;
            BL_Move = true;
        }

        if (BL_Alerted && BL_Move)
        {
            if (Vector3.Distance(Target.transform.position, transform.position) < 6.0f)
                RunFrom();
            else if (Vector3.Distance(Target.transform.position, transform.position) >= 6.0f)
                ChaseTo();
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
}
