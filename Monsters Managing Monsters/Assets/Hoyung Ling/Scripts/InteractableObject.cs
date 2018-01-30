using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InteractableObject
{
    public GameObject target;
    public float interactDistance;
    [TextArea(2, 10)]
    public string text;
    public bool acquired = false;
    public static bool BL_ShowDialogue = false;

    public GameObject exclaimationPoint;
    public GameObject interactionObject;

    public void CheckInteractState(GameObject PC)
    {
        if (target == null) return;

        CheckDistance(PC);
    }

    void CheckDistance(GameObject PC)
    {
        if (Vector3.Distance(PC.transform.position, target.transform.position) < interactDistance)
        {
            exclaimationPoint.SetActive(false);
            interactionObject.SetActive(true);
            if(Input.GetKeyDown(GameManager.instance.KC_Interact))
            {
                acquired = true;
            }
        }
        else
        {
            exclaimationPoint.SetActive(true);
            interactionObject.SetActive(false);
        }
    }
}
