using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float playerSpeed;
    private bool movementLock = false;
    private Vector3 mousePos;
    private Vector3 direction;
	
	// Update is called once per frame
	void Update () {

        if (!movementLock) CheckMovement();
	}

    private void CheckMovement()
    {
        // Input keys which translate player position relative to world space
        if (Input.GetKey(GameManager.instance.KC_Up))
        {
            transform.Translate(Vector3.up * playerSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(GameManager.instance.KC_Down))
        {
            transform.Translate(Vector3.down * playerSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(GameManager.instance.KC_Left))
        {
            transform.Translate(Vector3.left * playerSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(GameManager.instance.KC_Right))
        {
            transform.Translate(Vector3.right * playerSpeed * Time.deltaTime, Space.World);
        }
    }

    public void LockMovement(bool flag)
    {
        movementLock = flag;
    }

    public bool GetLockStatus()
    {
        return movementLock;
    }

}
