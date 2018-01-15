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
    public bool pauseMovement = false;
    public bool isIdle = true;                        //Should I idle?
    [Tooltip("Should the entity return to home when idle is off?")]

    public GameObject home;
    public Square[] Bounds;
    GameObject destination;

    float delay = 0;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Bounds.Length == 0)
            return;

        if (pauseMovement == true)
            return;

        if (isIdle)
        {
            if (Time.time > delay)
            {
                if (destination == null || Vector3.Distance(transform.position, destination.transform.position) < 0.1f)
                    NewPosition();
            }
        }
        else
            destination = home;

        if(destination != null)
            agent.destination = destination.transform.position;
    }

    void NewPosition()
    {
        Square square = Bounds[Random.Range(0, Bounds.Length - 1)];
        destination = square.Vertices[Random.Range(0, square.Vertices.Length)].Corner;
        delay = Time.time + square.Vertices[Random.Range(0, square.Vertices.Length)].idleDuration;
    }
}
