using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public string damagedAudio;

    public int health;
    int max_health;

    // Use this for initialization
    void Start()
    {
        max_health = health;
    }

    void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        AudioManager.instance.Play(damagedAudio);

    }

}
