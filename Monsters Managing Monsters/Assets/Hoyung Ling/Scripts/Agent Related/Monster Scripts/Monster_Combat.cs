using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Monster_Combat : MonoBehaviour {

    [HideInInspector]
    public GameObject Target;

    //Important variables
    NavMeshAgent enemy;

    public bool BL_InitiateCombat = false;

    [Header("Movement Params")]
    public float moveSpeed;
    bool BL_Move = false;

    [Header("Damage Prefabs (where applicable)")]
    // -------- Prefabs for damage dealing (where applicable) ------------
    public GameObject rangeAttack;
    public GameObject meleeAttack;

    // -------- Forward direction (towards the target) (for projectiles) ------------
    public GameObject GO_EjectionPoint;

    [Header("What distance to begin attacking")]
    // -------- RANGE ------------
    public float aggroRange;
    public float meleeRange;
    public float rangedRange;
    bool BL_Attack = false;

    [Header("Cooldown between attacks")]
    // -------- COOLDOWNS ------------ ------------ ------------ ------------ ------------
    public float rangeCooldown = 3f;       //Value added to Time.time which creates FL_Shoot, to determine the space between shoots
    private float FL_rangeInterval = 0;             //The value which Time.time compares itself to to know when to shoot

    public float meleeCooldown = 2f;
    private float FL_meleeInterval = 0;             //The value which Time.time compares itself to to know when to shoot

    // Use this for initialization
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Target == null) return;

        if (BL_InitiateCombat && Vector3.Distance(Target.transform.position, transform.position) < aggroRange)
        {
            BL_Move = true;
            BL_Attack = true;
            FL_meleeInterval = Time.time + meleeCooldown;
            FL_rangeInterval = Time.time + rangeCooldown;
        }

        if (BL_InitiateCombat)
        {
            if (BL_Move && Vector3.Distance(Target.transform.position, transform.position) >= 2.0f)
                ChaseTo();

            if (meleeAttack != null && BL_Attack && Vector3.Distance(Target.transform.position, transform.position) <= meleeRange)
                MeleeAttack();

            if (rangeAttack != null && BL_Attack && Vector3.Distance(Target.transform.position, transform.position) <= rangedRange)
                RangedAttack();
        }
    }

    //Chase Target
    void ChaseTo()
    {
        enemy.destination = Target.transform.position;
        enemy.speed = moveSpeed;
    }

    void MeleeAttack()
    {
        SpawnGO(FL_meleeInterval, meleeCooldown, meleeAttack);
    }

    void RangedAttack()
    {
        SpawnGO(FL_rangeInterval, rangeCooldown, rangeAttack);
    }

    void SpawnGO(float interval, float cooldown, GameObject spawnObject)
    {
        if (Time.time > interval)                                                                               //If the time is right...
        {
            Instantiate(spawnObject, 
                GO_EjectionPoint.transform.position, 
                GO_EjectionPoint.transform.rotation);                  //Spawn a bullet
            interval = Time.time + cooldown;                                                                 //Add a cooldown
        }
    }
}
