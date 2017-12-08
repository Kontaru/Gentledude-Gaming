using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Controller : MonoBehaviour
{

    public GameObject PC;

    Boss_Movement CC_Movement;
    Boss_Attack CC_Attack;

    //-----------------------------------------------------------
    public GameObject GO_Billboard;
    BossBillboard EB_Sprite;

    // Use this for initialization
    void Start()
    {
        EB_Sprite = GO_Billboard.GetComponent<BossBillboard>();

        CC_Movement = GetComponent<Boss_Movement>();
        CC_Attack = GetComponent<Boss_Attack>();

        Boss_Movement.Target = PC;
    }

    // Update is called once per frame
    void Update()
    {

        if (CC_Movement.BL_Alerted)
        {
            transform.LookAt(PC.transform.position);
            EB_Sprite.BL_Spotted = true;
            CC_Attack.BL_Target = true;
        }
        else
            EB_Sprite.BL_Spotted = false;
    }
}
