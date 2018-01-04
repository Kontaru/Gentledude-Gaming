using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroEntry : MonoBehaviour {

    public static HeroEntry instance;

    public bool SpawnHeroes;

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
    }
}
