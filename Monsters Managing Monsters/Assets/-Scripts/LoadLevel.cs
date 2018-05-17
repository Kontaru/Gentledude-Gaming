using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

    public void LoadByIndex(int index)
    {
        GameManager.instance.LoadScene(index);
    }
}
