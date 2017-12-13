using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    //Should we look at the camera?
    public bool BL_LookAtCam = false;
    public bool BL_LookAtPC = false;
    float playerfollowSmoothSpeed = 2f;

    //Direction in front of the player
    public Transform Forward;

    // Use this for initialization

    // Update is called once per frame
    virtual public void Update()
    {
        if(TargetHandler.instance.heroCount > 0)
        {
            BL_LookAtPC = false;
        }

        if(BL_LookAtPC)
        {
            var lookPos = transform.position - FindObjectOfType<PC_Controller>().gameObject.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, playerfollowSmoothSpeed * Time.deltaTime);
        }
        else if (BL_LookAtCam == true)
        {
            var lookPos = transform.position - Camera.main.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, playerfollowSmoothSpeed * Time.deltaTime);
        }
        else
        {
            var lookPos = transform.position - Forward.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, playerfollowSmoothSpeed * Time.deltaTime);
        }
    }
}
