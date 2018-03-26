using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MinorFlowchartInstance : MonoBehaviour {

    public static Flowchart instance;

    #region Typical Singleton Format

    void Awake()
    {
        //Singleton stuff
        if (instance == null)
            instance = this.GetComponent<Flowchart>();
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion
}
