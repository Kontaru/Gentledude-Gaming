﻿using System.Collections;
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
    private Vertice V_destination;
    GameObject GO_destination;

    public float FL_delay = 0;

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
        {
            NMA_agent.isStopped = true;
            return;
        }else
        {
            NMA_agent.isStopped = false;
        }

        if (BL_isIdle)
        {
            if (GO_destination == null || Vector3.Distance(transform.position, GO_destination.transform.position) < 0.1f)
            {
                if (Time.time > FL_delay)
                {
                    NewPosition();
                }
            }else
                FL_delay = Time.time + V_destination.idleDuration;

        }
        else
            GO_destination = GO_home;

        if(GO_destination != null)
            NMA_agent.destination = GO_destination.transform.position;
    }

    void NewPosition()
    {
        Square square = Bounds[Random.Range(0, Bounds.Length)];
        V_destination = square.Vertices[Random.Range(0, square.Vertices.Length)];
        GO_destination = V_destination.Corner;
    }
}
