using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLunge : MonoBehaviour
{

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject != null && coll.gameObject.tag == "Player")
        {
            coll.gameObject.SendMessage("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AudioManager.instance.Play("Player Slash");
    }

    void Update()
    {
        Destroy(gameObject, 0.2f);
    }
}
