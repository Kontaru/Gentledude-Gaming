using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBillboard : Billboard
{

    public bool BL_Spotted = false;
    bool BL_HasSpotted = false;
    bool BL_PermaOff = false;
    // Update is called once per frame
    override public void Update()
    {
        base.Update();
        if (BL_Spotted == true)
        {
            BL_LookAtCam = true;
            if (BL_HasSpotted == false && BL_PermaOff == false)
            {
                BL_HasSpotted = true;
            }
        }
        else
        {
            BL_PermaOff = false;
            BL_HasSpotted = false;
            BL_LookAtCam = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (BL_HasSpotted)
            StartCoroutine(Spotted(2.0f));
    }

    IEnumerator Spotted(float delay)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        BL_HasSpotted = false;
        BL_PermaOff = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
