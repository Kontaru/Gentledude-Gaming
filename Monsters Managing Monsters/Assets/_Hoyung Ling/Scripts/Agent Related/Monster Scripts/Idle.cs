using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Vertice
{
    public float idleDuration = 0;
    public GameObject Corner;
}

[System.Serializable]
public class Square
{
    public Vertice[] Vertices = new Vertice[4];
}

public class Idle : MonoBehaviour
{

    // -- For User Adjustment --
    public bool BL_pauseMovement = false;
    public bool BL_isIdle = true;                        //Should I idle?
    [Tooltip("Should the entity return to home when idle is off?")]

    public GameObject GO_home;
    public Square[] Bounds;
    GameObject GO_destination;

    float FL_delay = 0;

    NavMeshAgent NMA_agent;

    void Start()
    {
        NMA_agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Bounds.Length == 0)
            return;

        if (BL_pauseMovement == true)
            return;

        if (BL_isIdle)
        {
            if (Time.time > FL_delay)
            {
                if (GO_destination == null || Vector3.Distance(transform.position, GO_destination.transform.position) < 0.1f)
                    NewPosition();
            }
        }
        else
            GO_destination = GO_home;

        if(GO_destination != null)
            NMA_agent.destination = GO_destination.transform.position;
    }

    void NewPosition()
    {
        Square square = Bounds[Random.Range(0, Bounds.Length - 1)];
        GO_destination = square.Vertices[Random.Range(0, square.Vertices.Length)].Corner;
        FL_delay = Time.time + square.Vertices[Random.Range(0, square.Vertices.Length)].idleDuration;
    }
}
