using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBillboard : Billboard
{

    bool TriggeredOnce = false;

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
        if (BL_LookAtCam == true)
        {
            if (TriggeredOnce == false)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                TriggeredOnce = true;
            }
        }
        else
            transform.GetChild(0).gameObject.SetActive(false);
    }
}
