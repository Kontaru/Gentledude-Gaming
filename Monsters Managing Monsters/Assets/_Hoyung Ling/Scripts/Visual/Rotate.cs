using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public bool rotate_x = false;
    public bool rotate_y = true;
    public bool rotate_z = false;
    public float degree;

    float degree_x = 0;
    float degree_y = 0;
    float degree_z = 0;

    void Start()
    {
        if (rotate_x)
            degree_x = degree;
        if (rotate_y)
            degree_y = degree;
        if (rotate_z)
            degree_z = degree;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(degree_x, degree_y, degree_z));
    }
}
