using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {

    // -- For User Adjustment --
    public float scale;
    public float rate;

    //Target graphic
    Transform target;

    // -- Parameters --
    Vector3 V_DefaultScale;
    Vector3 V_NewScale;
    Vector3 V_ScaleBy = new Vector3(0.01f, 0.01f, 0f);

    [HideInInspector]
    public bool BL_Pulse = true;                                //Should the thing be pulsing? NOTE: Altered by other scripts
    bool BL_Swap = true;

	void Start () {
        target = transform.GetChild(0);
        V_DefaultScale = target.localScale;
        V_NewScale = new Vector3(target.localScale.x * scale, target.localScale.y * scale, 1);
	}
	
	// Update is called once per frame
	void Update () {

        if (!BL_Pulse)
        {
            target.gameObject.SetActive(false);
            return;
        }

        //This checks whether to pulse in or out
        if (target.localScale.x <= V_DefaultScale.x)
            BL_Swap = true;
        else if (target.localScale.x >= V_NewScale.x)
            BL_Swap = false;

        //After checking whether to pulse in or out, pulse appropriately
        if (BL_Swap == true)
            target.localScale += V_ScaleBy * rate;
        else if (BL_Swap == false)
            target.localScale -= V_ScaleBy * rate;
    }
}
