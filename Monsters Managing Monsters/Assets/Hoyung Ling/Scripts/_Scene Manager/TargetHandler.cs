using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : MonoBehaviour {

    public static TargetHandler instance;

    public GameObject[] monsters = new GameObject[3];
    public GameObject[] heroes;

    public int monsterCount = 0;                    //Number of enemies we have. Is increased each time an NPC is spawned
    public int heroCount = 0;

    public static bool heroesSpawned = false;

    bool heroesDead = false;

    #region Typical Singleton Format

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion 
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.PixelMode) return;

        if (heroesSpawned)
            StartCoroutine(CheckHeroes());

        StartCoroutine(CheckMonsters());
    }

    IEnumerator CheckMonsters()
    {
        yield return new WaitForSeconds(5.0f);

        int monsterDeathCount = 0;
        foreach (GameObject monster in monsters)
        {
            if (monster == null)
                monsterDeathCount++;
        }

        if (monsterDeathCount == monsters.Length)
        {
            GameManager.instance.LastScene();
        }
    }

    IEnumerator CheckHeroes()
    {
        yield return new WaitForSeconds(5.0f);

        int heroDeathCount = 0;
        foreach (GameObject hero in heroes)
        {
            if (hero == null)
                heroDeathCount++;
        }

        if (heroDeathCount == heroes.Length)
        {
            
        }
    }
}
