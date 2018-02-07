using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBillboard : Billboard
{

    public bool BL_Spotted = false;
    public GameObject Health;
    public GameObject CoinText;
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
            CoinText.SetActive(false);
            Health.SetActive(false);
        }

        if (BL_HasSpotted)
            StartCoroutine(Spotted(2.0f));
    }

    IEnumerator Spotted(float delay)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        CoinText.SetActive(true);
        Health.SetActive(true);


        yield return new WaitForSeconds(delay);
        CoinText.SetActive(false);
        BL_HasSpotted = false;
        BL_PermaOff = true;
    }
}
