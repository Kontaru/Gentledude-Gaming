using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Audio : MonoBehaviour
{

    PC_Controller CC_Controller;
    PC_Health CC_Health;
    bool sound_Move = true;
    bool sound_Stagger = true;

    // Use this for initialization
    void Start()
    {
        CC_Controller = GetComponent<PC_Controller>();
    }

    // Update is called once per frame
    void Update()
    {

        if (CC_Controller)

            if (CC_Controller.BL_IsMoving)
            {
                if (sound_Stagger == true && CC_Controller.BL_Staggered == true)
                {
                    AudioManager.instance.Play("Slow Footsteps");
                    AudioManager.instance.Stop("Footsteps");
                    sound_Stagger = false;
                    sound_Move = true;
                }


                if (sound_Move && CC_Controller.BL_Staggered == false)
                {
                    AudioManager.instance.Play("Footsteps");
                    AudioManager.instance.Stop("Slow Footsteps");
                    sound_Move = false;
                    sound_Stagger = true;
                }
            }
            else
            {
                sound_Move = true;
                sound_Stagger = true;
                AudioManager.instance.Stop("Footsteps");
                AudioManager.instance.Stop("Slow Footsteps");
            }
    }
}
