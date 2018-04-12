using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimate : MonoBehaviour {

    private Text label;
    private string message;

	// Use this for initialization
	void OnEnable () {
        label = GetComponent<Text>();
        message = label.text;
        StartCoroutine(AnimateText());
	}
	
    IEnumerator AnimateText()
    {
        label.text = "";

        foreach (char c in message)
        {
            label.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
