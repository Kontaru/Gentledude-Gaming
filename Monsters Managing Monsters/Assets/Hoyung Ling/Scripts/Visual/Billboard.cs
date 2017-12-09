using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    //Should we look at the camera?
    public bool BL_LookAtCam = false;

    //Direction in front of the player
    public Transform Forward;

    // Use this for initialization

    // Update is called once per frame
    virtual public void Update()
    {
        if (BL_LookAtCam == true)
        {
            transform.LookAt(Camera.main.transform);
        }
        else transform.LookAt(Forward);
    }
}
