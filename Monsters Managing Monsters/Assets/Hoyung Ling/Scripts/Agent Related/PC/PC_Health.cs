using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PC_Health : MonoBehaviour
{

    public int loadindex;
    public int health = 4;
    public static int max_health;

    // Use this for initialization
    void Start()
    {
        GameManager.instance.player_Health = health;
        max_health = health;
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        GameManager.instance.player_Health = health;

        if (health <= 0)
        {
            GameManager.instance.LoadScene(loadindex);
        }

        AudioManager.instance.Play("BossDamage");
    }

    void TakeHealth(int heal)
    {
        health += heal;
        AudioManager.instance.Play("Heal");
    }
}
