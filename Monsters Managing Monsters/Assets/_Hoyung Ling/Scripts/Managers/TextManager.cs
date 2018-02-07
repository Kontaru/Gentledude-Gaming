using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TextManager : MonoBehaviour {

    public Dialogue[] dialogues;            //An array of dialogue files
    Text textBox;
    GameObject textGO;

    float lerpTime = 0;
    public static bool showText = false;
    public static TextManager instance;

    #region Typical Singleton Format

    void Awake()
    {
        //Singleton stuff
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    private void Update()
    {
        if (textGO != null)
        {
            if (showText)
            {
                while (lerpTime < 1)
                {
                    lerpTime += Time.deltaTime;
                }
            }
            else if (!showText)
            {
                while (lerpTime > 0)
                {
                    lerpTime -= Time.deltaTime;
                }
            }

            textGO.GetComponent<RectTransform>().localPosition = Vector3.Lerp(
                new Vector3(0, -275, 0),
                new Vector3(0, -175, 0),
                lerpTime);
        }
    }

    //PRINTTEXT - Prints text by name. Use TextManager.instance.PrintText() to print from a text file.
    public string PrintText(string name)
    {
        Dialogue d = Array.Find(dialogues, dialogue => dialogue.name == name);
        if (d == null)
        {
            Debug.LogWarning("Text: " + name + " not found!");
            return null;
        }
        string dialogLines = d.textAsset.text;
        return dialogLines;
    }

    //DialogueBox - Prints text by name, to a specific DialogueBox. Use TextManager.instance.DialogueBox() to print from a text file.
    //Attach the TextBox script to the main dialogue box to assign itself to the textmanager
    public void DialogueBox(string name)
    {
        Dialogue d = Array.Find(dialogues, dialogue => dialogue.name == name);
        if (d == null)
        {
            Debug.LogWarning("Text: " + name + " not found!");
            return;
        }
        string dialogLines = d.textAsset.text;
        textBox.text = string.Format(dialogLines);
    }

    public void CustomText(string text)
    {
        textBox.text = string.Format(text);
    }

    public void textGO_Setter(GameObject GO)
    {
        textGO = GO;
    }

    //Don't worry about this one. It's just an identifier used by MainTextBox
    public void textBoxSetter(Text box)
    {
        textBox = box;
    }

    //Tells us who the textbox is
    public Text textBoxGetter()
    {
        return textBox;
    }
}
