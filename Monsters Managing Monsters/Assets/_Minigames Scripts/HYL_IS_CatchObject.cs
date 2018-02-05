using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HYL_IS_CatchObject : MonoBehaviour {

    float speed = 20.0f;

    NavMeshAgent nav_agent;
    GameObject PC;

    public GameObject[] destination;
    public int IN_currentDestination = 0;

    bool BL_Capturable = true;
    bool BL_Found = true;
    public bool BL_Captured = false;
    public enum StateMachine
    {
        None,
        Run,
        Found,
        Captured
    }

    public StateMachine State;

    // Use this for initialization
    void Start () {
        PC = GameManager.instance.Player;

        nav_agent = GetComponent<NavMeshAgent>();
        nav_agent.speed = speed;

        SafetyCooldown(5.0f);
    }
	
	// Update is called once per frame
	void Update () {

        CheckState();

        nav_agent.destination = destination[IN_currentDestination].transform.position;
    }

    void CheckState()
    {
        if (State == StateMachine.Run) Run();
        else if (State == StateMachine.Found) Found();
        else if (State == StateMachine.Captured) Captured();
    }

    void Run()
    {
        nav_agent.speed = speed;

        if (Vector3.Distance(transform.position, PC.transform.position) < 15f)
        {
            BL_Found = true;
            if (BL_Capturable == true)
            {
                if (IN_currentDestination >= destination.Length)    State = StateMachine.Captured;
                else                                                State = StateMachine.Found;
            }
        }

        if(Vector3.Distance(transform.position, destination[IN_currentDestination].transform.position) < 15f)
        {
            //run in circles
        }
    }

    void Found()
    {
        nav_agent.speed = 0;

        if (BL_Found)
        {
            IN_currentDestination++;
            BL_Found = false;
        }
        BL_Capturable = false;

        StartCoroutine(SafetyCooldown(6.0f));
        StartCoroutine(ChangeStateAfterX(1, StateMachine.Run));
    }

    void Captured()
    {
        nav_agent.speed = 0;
        BL_Captured = true;
    }

    IEnumerator SafetyCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        BL_Capturable = true;
    }

    IEnumerator ChangeStateAfterX(float seconds, StateMachine vState)
    {
        yield return new WaitForSeconds(seconds);
        State = vState;
    }
}
