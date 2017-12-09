using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU_Controller : MonoBehaviour {

    public static int npcCount = 0;               //Number of enemies we have. Is increased each time an NPC is spawned
    // Use this for initialization
    void Awake()
    {
        //Add our enemy to our count, then immediately reinitialise the list
        npcCount++;
        GameManager.instance.heroTargets = new GameObject[npcCount];
    }

    void Start () {
        HitList();
	}
	
	// Update is called once per frame
	void Update () {
	}

    #region Adds this NPC to the hitlist for the Hero

    void HitList()
    {
        for (int i = 0; i < GameManager.instance.heroTargets.Length; i++)
        {
            if (GameManager.instance.heroTargets[i] == null)
            {
                GameManager.instance.heroTargets[i] = gameObject;
                return;
            }
        }
    }

    #endregion
}
