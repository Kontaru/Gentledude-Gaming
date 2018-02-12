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

    float scale_x = 0;
    float scale_y = 0;
    float scale_z = 0;

    public bool bl_scale_x;
    public bool bl_scale_y;
    public bool bl_scale_z;

    [HideInInspector]
    public bool BL_Pulse = true;                                //Should the thing be pulsing? NOTE: Altered by other scripts
    bool BL_Swap = true;

	void Start () {
        target = transform.GetChild(0);
        V_DefaultScale = target.localScale;

        scale_x = 0;
        scale_y = 0;
        scale_z = 0;

        if (bl_scale_x)
            scale_x = target.localScale.x * scale;

        if (bl_scale_y)
            scale_y = target.localScale.y * scale;

        if (bl_scale_z)
            scale_z = target.localScale.z * scale;

        V_NewScale = new Vector3(scale_x, scale_y, scale_z);
        V_ScaleBy = new Vector3(scale_x / 100, scale_y / 100, scale_z / 100);
    }
	
	// Update is called once per frame
	void Update () {

        if (!BL_Pulse)
        {
            target.gameObject.SetActive(false);
            return;
        }

        //This checks whether to pulse in or out
        if (bl_scale_x)
        {
            if (target.localScale.x <= V_DefaultScale.x)
                BL_Swap = true;
            else if (target.localScale.x >= V_NewScale.x)
                BL_Swap = false;
        }
        else if (bl_scale_y)
        {
            if (target.localScale.y <= V_DefaultScale.y)
                BL_Swap = true;
            else if (target.localScale.y >= V_NewScale.y)
                BL_Swap = false;
        }else
        {
            if (target.localScale.z <= V_DefaultScale.z)
                BL_Swap = true;
            else if (target.localScale.z >= V_NewScale.z)
                BL_Swap = false;
        }


        //After checking whether to pulse in or out, pulse appropriately
        if (BL_Swap == true)
            target.localScale += V_ScaleBy * rate;
        else if (BL_Swap == false)
            target.localScale -= V_ScaleBy * rate;
    }
}
