using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HYL_IS_PigmanJim : MonoBehaviour {

    public float speed;
    bool BL_StealOnce = true;
    NavMeshAgent nav_agent;

    public GameObject parentedQuest;
    HYL_IS_Distribution quest;

    public GameObject PC;

    public enum StateMachine
    {
        None,
        Chase,
        Munch,
        Steal
    }

    public StateMachine State;

    void Start()
    {
        quest = parentedQuest.GetComponent<HYL_IS_Distribution>();
        PC = GameManager.instance.Player;

        nav_agent = GetComponent<NavMeshAgent>();
        nav_agent.speed = speed;
    }

    void Update()
    {
        CheckState();

        nav_agent.destination = PC.transform.position;
    }

    void CheckState()
    {
        if (State == StateMachine.Chase) Chase();
        else if (State == StateMachine.Munch) Munch();
        else if (State == StateMachine.Steal) StealDonut();
    }

    void Chase()
    {
        BL_StealOnce = true;

        if (Vector3.Distance(transform.position, PC.transform.position) < 15f)
            State = StateMachine.Steal;
    }

    void StealDonut()
    {
        nav_agent.speed = 0;

        if (BL_StealOnce)
        {
            quest.IN_ItemCount--;
            BL_StealOnce = false;
        }

        StartCoroutine(ChangeStateAfterX(3, StateMachine.Munch));
    }

    void Munch()
    {
        nav_agent.speed = speed;
        StartCoroutine(ChangeStateAfterX(1, StateMachine.Chase));
    }

    IEnumerator ChangeStateAfterX(float seconds, StateMachine vState)
    {
        yield return new WaitForSeconds(seconds);
        State = vState;
    }
}
