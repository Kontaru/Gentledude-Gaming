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

    public void ZoomedOut()
    {
        StopAllCoroutines();

        if (Camera.main.orthographic)
        {
            StartCoroutine(ChangeCameraOrthSize(Camera.main.orthographicSize, maxCameraSize, 0));
            StartCoroutine(ChangeCameraOrthSize(Camera.main.orthographicSize, FL_originalCamSize, 6));
        }
    }

    IEnumerator ChangeCameraOrthSize(float originalValue, float destinationValue, int time)
    {
        yield return new WaitForSeconds(time);

        while (Camera.main.orthographicSize != destinationValue)
        {
            Camera.main.orthographicSize = Mathf.Lerp(originalValue, destinationValue, rate);
            rate += Time.deltaTime / 5;
            yield return null;
        }
        rate = 0;
        otherLook = null;
    }
}
