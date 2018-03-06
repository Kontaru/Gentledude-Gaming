using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeActiveState : MonoBehaviour {

    public GameObject Focus;
    public GameObject[] Targets;

    private void OnTriggerEnter(Collider coll)
    {
        Entity e_coll = coll.gameObject.GetComponent<Entity>();
        if (e_coll != null && e_coll.EntityType == Entity.Entities.Player)
        {
            Focus.SetActive(!Focus.activeInHierarchy);
            foreach (GameObject target in Targets)
            {
                bool state = target.activeInHierarchy;
                target.SetActive(!state);
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        Entity e_coll = coll.gameObject.GetComponent<Entity>();
        if (e_coll != null && e_coll.EntityType == Entity.Entities.Player)
        {
            Focus.SetActive(!Focus.activeInHierarchy);
            foreach (GameObject target in Targets)
            {
                bool state = target.activeInHierarchy;
                target.SetActive(!state);
            }
        }
    }
}
