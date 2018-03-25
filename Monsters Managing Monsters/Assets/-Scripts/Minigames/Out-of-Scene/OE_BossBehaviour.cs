using UnityEngine;
using System.Collections;

public class OE_BossBehaviour : MonoBehaviour
{
    public GameObject stateInactive, stateWarning, stateActive;
    public enum State { Inactive, Warning, Active };
    private State currentState;

    private OE_PlayerBehaviour PB;

    private void OnEnable()
    {
        PB = FindObjectOfType<OE_PlayerBehaviour>();
        StartCoroutine(StateSwitcher());
    }

    void Start()
    {
        currentState = State.Active; 
    }

    private int GetRandomSecondDelay()
    {
        int value = Random.Range(1, 3);
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
            default:
                break;
        }
    }

    IEnumerator StateSwitcher()
    {
        while (true)
        {

        }
        yield return new WaitForSeconds(4);
        PB.escapedCount++;
        Destroy(gameObject);
    }
}


