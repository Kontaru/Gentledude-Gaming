using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Move : MonoBehaviour {

    Rigidbody RB_PC;
    Vector3 direction;

    [Header("Movement")]
    public float FL_moveSpeed;
    float FL_defaultSpeed;

    // Use this for initialization
    void Start () {
        RB_PC = GetComponent<Rigidbody>();
        FL_defaultSpeed = FL_moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        FL_moveSpeed = FL_defaultSpeed;
        PlayerMove();
        //LookInput();
    }

    void FixedUpdate()
    {
        RB_PC.MovePosition(transform.position + direction * Time.fixedDeltaTime);
    }

    void PlayerMove()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        direction = moveInput.normalized * FL_moveSpeed;
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
