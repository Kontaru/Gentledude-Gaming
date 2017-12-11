using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : MonoBehaviour
{

    // -- For User Adjustment --
    public bool idle = true;                        //Should I idle?
    [Tooltip("Should the entity return to home when idle is off?")]
    public bool return_home = true;                  //Should I return home when idle is turned off? (for example, if 
    public bool pickNewPositions = true;             //If we want to stop 

    // -- Movement Params --
    public float moveSpeed;                          //How fast the entity paces around the room
    public GameObject[] Corners;                        //The bounds for the area that the player can move within
    Vector3[] V_Bounds;                              //The positions of the bounds. NOTE: V_Bounds[0]  will always be Home
    Vector3 V_MoveTo;                                //Entity's location that it wants to move to

    void Start()
    {

        //Set the size of the number of bounds, including the player's home position (+1)
        //We include the "home" position in our bounds, in the case where we only have 1 area gameobject (making two points to idle between)
        V_Bounds = new Vector3[Corners.Length + 1];

        int count = 1;
        foreach (GameObject corner in Corners)
        {
            V_Bounds[count] = corner.transform.position;
            count++;
        }

        V_Bounds[0] = transform.position;                       //Set the first vector to "home"
        V_MoveTo = V_Bounds[1];                                 //Just to set our NPC off
    }

    void Update()
    {

        //If we're idling, move about. If not, either we return home, or we stay in position
        if (idle)
            Move();
        else if (return_home)
            Vector2.Lerp(transform.position, V_Bounds[0], Time.deltaTime);
    }

    void Move()
    {
        //This does the actual motion
        transform.position = Vector2.Lerp(transform.position, V_MoveTo, Time.deltaTime);

        //Once the motion is over, we want to see where we want to go next
        if (Vector2.Distance(transform.position, V_MoveTo) < 0.1)
        {
            //If there's only two positions (the home position, and 1 other defined position), then don't bother calculating bounds!
            if (V_Bounds.Length == 2)
            {
                if (V_MoveTo == V_Bounds[0])
                    V_MoveTo = V_Bounds[1];
                else
                    V_MoveTo = V_Bounds[0];
            }
            else
                V_MoveTo = Reposition(V_Bounds);

        }

    }

    #region -- Position Calculations --

    Vector3 Reposition(Vector3[] area)
    {
        //Grab my bounds (nice and firm)
        float[] bounds = CalculateBounds(area);

        //Our bounds in a better more readable form
        float posXmin = bounds[0];
        float posYmin = bounds[1];
        float posXmax = bounds[2];
        float posYmax = bounds[3];

        Vector3 position = new Vector3();
        float newX = 0;
        float newY = 0;

        // ~~~~~ BEGIN ACTUAL CODE ~~~~~

        newX = Random.Range(posXmin, posXmax);
        newY = Random.Range(posYmin, posYmax);

        return position;
    }

    float[] CalculateBounds(Vector3[] corner)
    {
        //An array for the possible mins and maxes - we'll return this value later
        float[] bounds = new float[4];

        //For the intent of the calculation (and to make it more readable), I've adopted this approach to represent min and max values for now
        float posXmin = corner[0].x;
        float posYmin = corner[0].y;
        float posXmax = corner[0].x;
        float posYmax = corner[0].y;

        //These forloops will check each of our bounds, and pick the smallest/largest number
        #region Minimum Values
        //Min X
        for (int i = 0; i < corner.Length; i++)
        {
            if (posXmin > corner[i].x)
                posXmin = corner[i].x;
        }

        //Min Y
        for (int i = 0; i < corner.Length - 1; i++)
        {
            if (posYmin > corner[i].y)
                posYmin = corner[i].y;
        }
        #endregion

        #region Maximum Values
        //Max X
        for (int i = 0; i < corner.Length - 1; i++)
        {
            if (posXmax < corner[i].x)
                posXmax = corner[i].x;
        }
        //Max Y
        for (int i = 0; i < corner.Length - 1; i++)
        {
            if (posYmax < corner[i].y)
                posYmax = corner[i].y;
        }
        #endregion

        bounds[0] = posXmin;
        bounds[1] = posYmin;
        bounds[2] = posXmax;
        bounds[3] = posYmax;
        return bounds;
    }

    void Pickapoint()
    {
        //Pick two random corners
    }

    #endregion
}
