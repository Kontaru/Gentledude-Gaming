using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU_Controller : MonoBehaviour {

    public bool Destroyme;
	// Use this for initialization
	void Start () {
        HitList();
	}
	
	// Update is called once per frame
	void Update () {
        if (Destroyme)
            Destroy(gameObject);
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

            if (i == GameManager.instance.heroTargets.Length - 1)
            {
                GameObject[] tArray = new GameObject[GameManager.instance.heroTargets.Length];
                for (int k = 0; k < GameManager.instance.heroTargets.Length; k++)
                {
                    tArray[k] = GameManager.instance.heroTargets[k];
                }
                GameManager.instance.heroTargets = new GameObject[GameManager.instance.heroTargets.Length + 1];

                for (int k = 0; k < GameManager.instance.heroTargets.Length - 1; k++)
                {
                    GameManager.instance.heroTargets[k] = tArray[k];
                }

                GameManager.instance.heroTargets[GameManager.instance.heroTargets.Length] = gameObject;
            }
        }
    }

    #endregion
}
