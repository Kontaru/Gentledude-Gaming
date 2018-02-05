using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
class Popup
{
    public string name;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public TextMeshProUGUI popup;
    [TextArea(2, 10)]
    public string message;
}

class MessagePopup : MonoBehaviour
{

    public float interactableDist;
    float FL_P1_dist;

    public Popup[] Notice;
    int count = 0;

    GameObject indicator;

    void Start()
    {
        foreach (Popup var in Notice)
        {
            var.target = transform.GetChild(count + 1).gameObject;
            var.popup = var.target.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var.popup.text = string.Format(var.message);

            count++;
        }

        indicator = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        FL_P1_dist = Vector3.Distance(transform.position, GameManager.instance.Player.transform.position);

        if (FL_P1_dist < interactableDist)
        {
            foreach (Popup popup in Notice)
            {
                popup.target.SetActive(true);
            }
            if(indicator != null) indicator.SetActive(false);
        }
        else
        {
            foreach (Popup popup in Notice)
            {
                popup.target.SetActive(false);
            }
            if (indicator != null) indicator.SetActive(true);
        }
    }
}
