using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Dialogue : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showTextBox()
    {
        //TextManager.instance.textBoxGetter;
    }

    public void SetText(string text)
    {
        TextManager.instance.CustomText(text);
    }
}
