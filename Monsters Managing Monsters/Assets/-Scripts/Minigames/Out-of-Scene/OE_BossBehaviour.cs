using UnityEngine;
using System.Collections;

public class OE_BossBehaviour : MonoBehaviour
{
    public GameObject stateInactive, stateWarning, stateActive;
    public enum State { Inactive, Warning, Active };
    private State currentState;

    private OE_PlayerBehaviour PB;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Worker(Clone)")
        {
            if (currentState == State.Active) Invoke("SeenWorker", 1f);
        }
    }

    private void OnEnable()
    {
        PB = FindObjectOfType<OE_PlayerBehaviour>();

        currentState = State.Active;
        SetState(currentState);

        StartCoroutine(StateSwitcher());
    }

    private int GetRandomSecondDelay(int min, int max)
    {
        int value = Random.Range(min, max);
        return value;
    }

    private void HideAllStates()
    {
        stateInactive.SetActive(false);
        stateWarning.SetActive(false);
        stateActive.SetActive(false);
    }

    private void SetState(State state)
    {
        HideAllStates();
        currentState = state;

        switch (state)
        {
            case State.Inactive:
                stateInactive.SetActive(true);
                break;
            case State.Warning:
                stateWarning.SetActive(true);
                break;
            case State.Active:
                stateActive.SetActive(true);
                break;
        }
    }

    private void SeenWorker()
    {
        PB.BL_MinigameFailed = true;
    }

    IEnumerator StateSwitcher()
    {
        yield return new WaitForSeconds(GetRandomSecondDelay(3, 8));
        SetState(State.Inactive);
        yield return new WaitForSeconds(GetRandomSecondDelay(2, 6));
        SetState(State.Warning);
        yield return new WaitForSeconds(4);
        SetState(State.Active);

        StartCoroutine(StateSwitcher());
    }
}


