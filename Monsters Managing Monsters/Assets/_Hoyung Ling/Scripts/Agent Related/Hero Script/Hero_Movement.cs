using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Hero_Movement : MonoBehaviour {

    [HideInInspector]
    public GameObject Target;

    [Header("Direction")]
    public Transform Behind;

    //Important variables
    NavMeshAgent enemy;

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
        if (GameManager.instance.PixelMode) return;

        if (Target == null) return;

        if (!BL_Alerted && Vector3.Distance(Target.transform.position, transform.position) < 4000.0f)
        {
            BL_Alerted = true;
            BL_Move = true;
        }

        if (BL_Alerted && BL_Move)
        {
            if (Vector3.Distance(Target.transform.position, transform.position) < 2.0f)
                RunFrom();
            else if (Vector3.Distance(Target.transform.position, transform.position) >= 2.0f)
                ChaseTo();
        }
    }

    //Chase Target
    public void ChaseTo()
    {
        enemy.destination = Target.transform.position;
        enemy.speed = moveSpeed;
    }

    //Back up from Target
    public void RunFrom()
    {
        enemy.destination = Behind.position;
        enemy.speed = moveSpeed * 4.0f;
    }
}
