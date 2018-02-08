using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour {

    public bool offset_x = false;
    public bool offset_y = true;
    public bool offset_z = false;

    public float rate;
    float interval = 0;

    public float degree_x = 0;
    public float degree_y = 0;
    public float degree_z = 0;

    Vector3 home;
    Vector3 destination;

    // Use this for initialization
    void Start () {
        if (!offset_x)
            degree_x = transform.localPosition.x;
        if (!offset_y)
            degree_y = transform.localPosition.y;
        if (!offset_z)
            degree_z = transform.localPosition.z;

        home = transform.localPosition;
        destination = new Vector3(transform.localPosition.x + degree_x,
            transform.localPosition.y + degree_y,
            transform.localPosition.z + degree_z);
    }
	
	// Update is called once per frame
	void Update () {

        transform.localPosition = Vector3.Lerp(home, destination, interval);

        interval += Time.deltaTime * rate;
        if (interval > 1 || interval < 0)
            rate = -rate;
    }
}
