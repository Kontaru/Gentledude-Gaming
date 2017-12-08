using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack : MonoBehaviour
{

    public GameObject Sword_Lunge;
    public GameObject PF_Bullet;
    public GameObject PF_Lunge;
    public GameObject GO_EjectionPoint;

    public bool BL_Target = false;                  //Do we target?
    private float FL_Shoot = 0;             //The value which Time.time compares itself to to know when to shoot
    protected float FL_CoolDown = 3f;       //Value added to Time.time which creates FL_Shoot, to determine the space between shoots

    private float FL_Lunge = 0;             //The value which Time.time compares itself to to know when to shoot
    protected float FL_LungeCoolDown = 5f;
    bool BL_LungeAnimator = false;

    bool BL_Spotted = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (BL_Target && !BL_Spotted)
        {
            FL_Lunge = Time.time + FL_LungeCoolDown;
            FL_Shoot = Time.time + FL_CoolDown;
            BL_Spotted = true;
        }

        FireAtPC();
        LungeAtPC();

    }

    #region --- Spawn Lunge ---

    void SpawnLunge()
    {
        Sword_Lunge.SetActive(false);
        if (!BL_LungeAnimator)
        {
            StartCoroutine(SwordSpriteAnimator());
            BL_LungeAnimator = true;
        }
        Instantiate(PF_Lunge, GO_EjectionPoint.transform.position, GO_EjectionPoint.transform.rotation);
    }

    #endregion

    #region --- Begin Lunging ---

    void LungeAtPC()
    {
        if (BL_Target)                                                                                              //If we're allowed to target
        {
            if (Time.time > FL_Lunge)                                                                               //If the time is right...
            {
                SpawnLunge();                                                                                   //Spawn a bullet
                FL_Lunge = Time.time + FL_LungeCoolDown;                                                                 //Add a cooldown
            }
        }
    }
    #endregion

    IEnumerator SwordSpriteAnimator()
    {
        yield return new WaitForSeconds(0.21f);
        BL_LungeAnimator = false;
        Sword_Lunge.SetActive(true);
    }

    #region --- Spawn Bullet ---

    //------------------------------------------------------------
    //Instantiates a bullet at the tip of the gun (just the tip)
    //------------------------------------------------------------
    protected void SpawnBullet()
    {
        //Generate a bullet at the ejection point and play some audio
        Instantiate(PF_Bullet, GO_EjectionPoint.transform.position, GO_EjectionPoint.transform.rotation);
    }

    #endregion

    #region --- Begin Shooting ---

    void FireAtPC()
    {
        if (BL_Target)                                                                                              //If we're allowed to target
        {
            if (Time.time > FL_Shoot)                                                                               //If the time is right...
            {
                SpawnBullet();                                                                                      //Spawn a bullet
                FL_Shoot = Time.time + FL_CoolDown;                                                                 //Add a cooldown
            }
        }
    }
    #endregion
}
