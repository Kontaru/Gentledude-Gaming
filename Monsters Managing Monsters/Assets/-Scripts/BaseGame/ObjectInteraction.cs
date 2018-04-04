using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour {

    public GameObject defaultObject, labelObject;
    public Text defaultText;
    public Text labelText;

    public string ST_default, ST_label;
    public bool BL_collectPDA;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") ShowLabel();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E)) Interact();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") ShowDefault();
    }

    private void Start()
    {
        defaultText.text = ST_default;
        labelText.text = ST_label;
        ShowDefault();
    }

    private void Interact()
    {
        if (BL_collectPDA) 
        {
            PDAHandler.instance.TogglePDA();
            EndDaySummary.instance.QuestCompleted("Collected PDA", false);
        }       
    }

    private void ShowDefault()
    {
        HideAll();
        defaultObject.SetActive(true);
    }

    private void ShowLabel()
    {
        HideAll();
        labelObject.SetActive(true);
    }

    private void HideAll()
    {
        defaultObject.SetActive(false);
        labelObject.SetActive(false);
    }
}
