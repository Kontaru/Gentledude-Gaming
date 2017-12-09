using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Health : MonoBehaviour
{

    public GameObject GO_Hatch;
    public enum State
    {
        Phase1,
        Phase2,
        None
    }

    public State CurrentState;

    public static int health = 16;
    int max_health;

    // Use this for initialization
    void Start()
    {
        max_health = health;
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health > 6 && health <= 16)
            CurrentState = State.Phase1;
        else if (health <= 6 && health > 0)
            CurrentState = State.Phase2;
        else if (health <= 0)
        {
            Destroy(gameObject);
        }

        int rand = Random.Range(0, 2);

        if (rand == 0)
            AudioManager.instance.Play("PC Damage 1");
        else if (rand == 1)
            AudioManager.instance.Play("PC Damage 2");
        else if (rand == 2)
            AudioManager.instance.Play("Metal Noise 2");

    }
}
