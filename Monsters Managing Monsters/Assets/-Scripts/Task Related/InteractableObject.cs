using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    [HideInInspector]
    public GameObject target;
    public float interactDistance;
    [TextArea(2, 10)]
    public string text;
    public bool acquired = false;
    public static bool BL_ShowDialogue = false;

    GameObject exclaimationPoint;
    GameObject interactionObject;

    public void Start()
    {
        target = transform.gameObject;
        exclaimationPoint = target.transform.GetChild(1).transform.GetChild(1).gameObject;
        interactionObject = target.transform.GetChild(1).transform.GetChild(0).gameObject;
    }

    public void Update()
    {
        CheckInteractState(GameManager.instance.Player);
        CheckDistance(GameManager.instance.Player);
    }

    public void CheckInteractState(GameObject PC)
    {
        if (Vector3.Distance(PC.transform.position, target.transform.position) < interactDistance)
        {
            exclaimationPoint.SetActive(false);
            interactionObject.SetActive(true);
        }
        else
        {
            exclaimationPoint.SetActive(true);
            interactionObject.SetActive(false);
        }
    }

    void CheckDistance(GameObject PC)
    {
        if (Vector3.Distance(PC.transform.position, target.transform.position) < interactDistance)
        {
            if(Input.GetKeyDown(GameManager.instance.KC_Interact))
            {
                FungusDirector.instance.IdleNPC(text);
                acquired = true;
            }
        }
    }
}
