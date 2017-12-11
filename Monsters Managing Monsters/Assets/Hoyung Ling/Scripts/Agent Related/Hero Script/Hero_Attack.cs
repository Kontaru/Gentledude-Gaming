using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Attack : MonoBehaviour
{
    [HideInInspector]
    public GameObject Target;

    // -------- Graphic for the weapon that the hero holds ------------
    public GameObject Sword_Graphic;

    [Header("Damage Prefabs (where applicable)")]
    // -------- Prefabs for damage dealing (where applicable) ------------
    public GameObject projectileAttack;
    public GameObject meleeAttack;

    // -------- Forward direction (towards the target) (for projectiles) ------------
    public GameObject GO_EjectionPoint;

    [Header("What distance to begin attacking")]
    // -------- RANGE ------------
    public float meleeRange;
    public float rangedRange;

    [Header("Cooldown between attacks")]
    // -------- COOLDOWNS ------------ ------------ ------------ ------------ ------------
    public float rangeCooldown = 3f;       //Value added to Time.time which creates FL_Shoot, to determine the space between shoots
    private float FL_rangeInterval = 0;             //The value which Time.time compares itself to to know when to shoot

    public float meleeCooldown = 2f;
    private float FL_meleeInterval = 0;             //The value which Time.time compares itself to to know when to shoot

    // -------- BOOLEANS ------------ ------------ ------------ ------------ ------------
    [HideInInspector]
    public bool BL_Target = false;          //Do we target?
    bool BL_LungeAnimator = false;
    bool BL_Spotted = false;                //If we have a target, we have spotted a target. The code should flip this to true.


    void Update()
    {
        //The moment we have a target, tell myself that I've spotted the target, and set some delays
        //This is so the NPC doesn't attack straight after spotting a target
        if (BL_Target && !BL_Spotted)
        {
            FL_meleeInterval = Time.time + meleeCooldown;
            FL_rangeInterval = Time.time + rangeCooldown;
            BL_Spotted = true;
        }

        if (Target != null)
        {
            //If the prefabs are in place, do the relevant attack
            if (projectileAttack != null && Vector3.Distance(Target.transform.position, transform.position) < rangedRange)
                FireAtPC();
            if (meleeAttack != null && Vector3.Distance(Target.transform.position, transform.position) < meleeRange)
                LungeAtPC();
        }

    }

    //------------------------------------------------------------
    //Instantiates a sword to deal damage, whilst making the sword disappear and reappear in the player's hand
    //------------------------------------------------------------

    #region --- Spawn Lunge ---

    //Simple method for spawning the melee
    void SpawnLunge()
    {
        //Set the graphic that the NPC is holding to false
        Sword_Graphic.SetActive(false);
        //Makes it so the sword reappears in their hand after a delay
        if (!BL_LungeAnimator)
        {
            StartCoroutine(SwordSpriteAnimator());
            BL_LungeAnimator = true;
        }

        //Spawns the actual sword to do the damage
        Instantiate(meleeAttack, GO_EjectionPoint.transform.position, GO_EjectionPoint.transform.rotation);
    }

    //Makes the sword sprite reappear
    IEnumerator SwordSpriteAnimator()
    {
        yield return new WaitForSeconds(0.21f);
        BL_LungeAnimator = false;
        Sword_Graphic.SetActive(true);
    }

    #endregion

    #region --- Begin Lunging ---

    void LungeAtPC()
    {
        if (BL_Target)                                                                                              //If we're allowed to target
        {
            if (Time.time > FL_meleeInterval)                                                                               //If the time is right...
            {
                SpawnLunge();                                                                                       //Spawn a bullet
                FL_meleeInterval = Time.time + meleeCooldown;                                                            //Add a cooldown
            }
        }
    }

    #endregion

    //------------------------------------------------------------
    //Instantiates a bullet at the tip of the gun (just the tip)
    //------------------------------------------------------------

    #region --- Spawn Bullet ---

    protected void SpawnBullet()
    {
        //Generate a bullet at the ejection point and play some audio
        Instantiate(projectileAttack, GO_EjectionPoint.transform.position, GO_EjectionPoint.transform.rotation);
    }

    #endregion

    #region --- Begin Shooting ---

    void FireAtPC()
    {
        if (BL_Target)                                                                                              //If we're allowed to target
        {
            if (Time.time > FL_rangeInterval)                                                                               //If the time is right...
            {
                SpawnBullet();                                                                                      //Spawn a bullet
                FL_rangeInterval = Time.time + rangeCooldown;                                                                 //Add a cooldown
            }
        }
    }

    #endregion
}
