using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{

    public bool BL_BelongsToEnemy = false;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject != null && coll.gameObject.tag != "Player" && !BL_BelongsToEnemy)
        {
            coll.gameObject.SendMessage("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
        else if (coll.gameObject != null && coll.gameObject.tag == "Player" && BL_BelongsToEnemy)
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
