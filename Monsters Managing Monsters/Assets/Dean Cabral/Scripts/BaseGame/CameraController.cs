using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

    [HideInInspector]
    public Vector3 camPosition;
    public Vector2 panLimit;
    public GameObject player;

    public float panBorder;
    public float panSpeed;
    public float panRestriction;
    public float scrollSpeed;

    private PC_Controller PM;
    private PDAHandler PDAH;
    private bool camLock;
    private bool camInMotion;
    private float orthographicSize;

	// Use this for initialization
	void Start () {

        Initialise();        
    }
	
	// Update is called once per frame
	void Update () {

        CheckInput();
        CameraMovement();

    }

    private void Initialise()
    {
        // Variables to be initilaised on start
        PM = FindObjectOfType<PC_Controller>();
        PDAH = FindObjectOfType<PDAHandler>();
        orthographicSize = Camera.main.orthographicSize;

        panBorder = 20f;
        panSpeed = 5f;
        panRestriction = 30f;
        panLimit.x = panRestriction;
        panLimit.y = panRestriction;
        scrollSpeed = 3f;
        
        camLock = true;
        camInMotion = false;
        
    }


    private void CheckInput()
    {
        if (Input.GetKeyDown(GameManager.instance.KC_CameraLock))
        {
            // Switches boolean state
            camLock = !camLock;

            // Freecam mode
            //if (camLock) PM.LockMovement(true);
            //else PM.LockMovement(false);

            // Resets the camera position on toggle (design decision?)
            StartCoroutine(MoveCamToPlayer());            
        }
    }

    private void CameraMovement()
    {
        //PanCamera();
        ZoomCamera();        
    }

    private void PanCamera()
    {
        if (PDAH.BL_PDAactive) return;
        // Prevents movement and camera panning happening at the same time
        if (PlayerInMotion()) return;
        //if (camInMotion) PM.LockMovement(true);
        //else PM.LockMovement(false);

        // Camera pan movement logic - uses camera border thickness to manipulate camera position
        camPosition = transform.position;

        if (Input.mousePosition.y >= Screen.height - panBorder)
        {
            camInMotion = true;
            camPosition.y += panSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.y <= panBorder)
        {
            camInMotion = true;
            camPosition.y -= panSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.x >= Screen.width - panBorder)
        {
            camInMotion = true;
            camPosition.x += panSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.x <= panBorder)
        {
            camInMotion = true;
            camPosition.x -= panSpeed * Time.deltaTime;
        }
        else
        {
            if (camLock) StartCoroutine(MoveCamToPlayer());
        }

        // Clamps the camera position to the specified pan limit vector 2
        camPosition.x = Mathf.Clamp(camPosition.x, -panLimit.x, panLimit.x);
        camPosition.y = Mathf.Clamp(camPosition.y, -panLimit.y, panLimit.y);

        transform.position = camPosition;
    }

    private void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            // Adjusts the orthographic size of the camera by the scroll amount
            orthographicSize -= scroll * scrollSpeed;
            // Clamps the minimum and maximum zoom distance
            orthographicSize = Mathf.Clamp(orthographicSize, 5.0f, 10.0f);
        }

        // Smooth value lerping of the main camera's orthographic size
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, orthographicSize, 2 * Time.deltaTime);
    }

    private bool PlayerInMotion()
    {
        // Input check for movement
        if (Input.GetKey(GameManager.instance.KC_Up)) return true;
        if (Input.GetKey(GameManager.instance.KC_Down)) return true;
        if (Input.GetKey(GameManager.instance.KC_Left)) return true;
        if (Input.GetKey(GameManager.instance.KC_Right)) return true;

        return false;
    }

    IEnumerator MoveCamToPlayer()
    {
        // Retrieves the player's target position relative to the camera
        Vector3 playerPos = player.transform.position;
        Vector3 targetPos = new Vector3(playerPos.x, playerPos.y, transform.position.z);
        camInMotion = true;

        while (transform.position != targetPos)
        {
            // Moves the camera position to the player's target position
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime);
            yield return null;
        }

        camInMotion = false;

    }

}