using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandler : MonoBehaviour {

    public static TargetHandler instance;

    public GameObject[] monsters = new GameObject[3];
    public GameObject[] heroes;

    public int monsterCount = 0;                    //Number of enemies we have. Is increased each time an NPC is spawned
    public int heroCount = 0;

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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
