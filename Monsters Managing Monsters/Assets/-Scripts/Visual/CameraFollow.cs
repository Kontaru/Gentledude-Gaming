using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    //Very simple code which makes an empty game object follow the player, 
    //To allow the main camera to tracking the player
    public static CameraFollow instance;
    public GameObject GO_Player;          //Reference to player
    public GameObject otherLook;

    public float smoothSpeed = 1f;
    public float playerfollowSmoothSpeed = 1f;

    public float maxCameraSize;
    private float FL_originalCamSize;

    public float rate;

    #region Typical Singleton Format

    void Awake()
    {
        //Singleton stuff
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        FL_originalCamSize = Camera.main.orthographicSize;
    }

    #endregion

    void Update()
    {
        PCFollowBody();
    }

    void PCFollowBody()
    {
        //Sets transform to the player
        if (otherLook == null)
        {
            if (Vector3.Distance(transform.position, GO_Player.transform.position) < 2.0f)
                transform.position = Vector3.Lerp(transform.position, GO_Player.transform.position, playerfollowSmoothSpeed * Time.deltaTime);
            else
                transform.position = Vector3.Lerp(transform.position, GO_Player.transform.position, smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, otherLook.transform.position, smoothSpeed * Time.deltaTime);
        }
    }

    void ZoomedOut(bool zoomOut)
    {
        if (zoomOut == true && Camera.main.orthographic)
        {
            StartCoroutine(ChangeCameraOrthSize(Camera.main.orthographicSize, maxCameraSize));
        }else if (zoomOut == false && Camera.main.orthographic)
        {
            StartCoroutine(ChangeCameraOrthSize(Camera.main.orthographicSize, FL_originalCamSize));
        }
    }

    IEnumerator ChangeCameraOrthSize(float originalValue, float destinationValue)
    {
        do
        {
            Camera.main.orthographicSize = Mathf.Lerp(originalValue, destinationValue, rate);
            rate += Time.deltaTime / 20;
            yield return null;
        } while (Camera.main.orthographicSize != destinationValue);
        rate = 0;
    }
}
