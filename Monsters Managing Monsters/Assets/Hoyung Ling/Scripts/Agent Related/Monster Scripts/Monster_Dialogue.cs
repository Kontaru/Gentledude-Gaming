using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Dialogue : MonoBehaviour {

    public static bool BL_ShowDialogue = false;
    public bool BL_SetText = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (BL_ShowDialogue)
        {
            TextManager.showText = true;
        }
        else
        {
            TextManager.showText = false;
        }

        BL_SetText = BL_ShowDialogue;
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
