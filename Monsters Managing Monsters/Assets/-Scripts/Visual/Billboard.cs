using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    //Should we look at the camera?
    public bool BL_LookAtCam = false;

    private GameObject player;
    private Quaternion originalRot;

    float playerfollowSmoothSpeed = 5f;
    Vector3 lookPos;
    Quaternion rotation;

    // Use this for initialization
    private void Start()
    {
        //player = FindObjectOfType<PC_Controller>().gameObject;
        originalRot = transform.rotation;
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (player != null && Vector3.Distance(gameObject.transform.position, player.transform.position) < 10f && !BL_LookAtCam)
        {
            //originalRot = transform.rotation;
            lookPos = transform.position - player.transform.position;
            lookPos.y = 0;
            if (lookPos != Vector3.zero)
                rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, playerfollowSmoothSpeed * Time.deltaTime);
        }else if (BL_LookAtCam)
        {
            if (Camera.main.orthographic)
            {
                //lookPos = transform.position - Camera.main.transform.position;
                //rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Euler((90f - Camera.main.transform.rotation.x), 0, 0);
            }
            else
            {
                //originalRot = transform.rotation;
                lookPos = transform.position - Camera.main.transform.position;
                rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, playerfollowSmoothSpeed * Time.deltaTime);
            }
        }
        else
        {
            lookPos = transform.position - Camera.main.transform.position;
            if (lookPos != Vector3.zero)
                rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, playerfollowSmoothSpeed * Time.deltaTime);
        }
    }
}
