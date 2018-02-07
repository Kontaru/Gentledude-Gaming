using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTextBox : MonoBehaviour {

    [Header("Attached to the main text box")]
    public bool Main = false;
    public GameObject body;

    private Text myText;

	void Start () {
        myText = GetComponent<Text>();
        body = transform.parent.gameObject;

        if (Main == true)
        {
            TextManager.instance.textBoxSetter(myText);
            TextManager.instance.textGO_Setter(transform.parent.gameObject);   
        }
	}
}
