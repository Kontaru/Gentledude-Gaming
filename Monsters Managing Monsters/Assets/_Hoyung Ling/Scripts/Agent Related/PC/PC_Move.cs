using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Move : MonoBehaviour {

    Rigidbody RB_PC;
    Vector3 V_direction;

    [Header("Movement")]
    public float FL_moveSpeed;
    public bool BL_isMoving = false;
    public static bool BL_canMove = true;
    float FL_defaultSpeed;

    // Use this for initialization
    void Start () {
        RB_PC = GetComponent<Rigidbody>();
        FL_defaultSpeed = FL_moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.PixelMode) return;

        FL_moveSpeed = FL_defaultSpeed;
        PlayerMove();
        //LookInput();
    }

    void FixedUpdate()
    {
        RB_PC.MovePosition(transform.position + V_direction * Time.fixedDeltaTime);
    }

    void PlayerMove()
    {
        if (!BL_canMove)
        {
            BL_isMoving = false;
            return;
        }

        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (moveInput == new Vector3(0, 0, 0))
            BL_isMoving = false;
        else
            BL_isMoving = true;

        V_direction = moveInput.normalized * FL_moveSpeed;
    }

    void LookInput()
    {
        //Look Input
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane grounPlane = new Plane(Vector3.up, Vector3.up);
        float rayDistance;

        if (grounPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 heightCorrectedPoint = new Vector3(point.x, transform.position.y, point.z);
            transform.LookAt(heightCorrectedPoint);
        }
    }
}
