using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_IS_DonutDistribution : HYL_IS_Distribution {

    public GameObject PigmanJim;
	
	// Update is called once per frame
	override public void Fluff() {
        if (BL_IsInteractable)
            PigmanJim.SetActive(true);
        else
            PigmanJim.SetActive(false);
    }
}
