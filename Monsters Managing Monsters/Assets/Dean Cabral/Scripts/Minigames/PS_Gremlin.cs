using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_Gremlin : MonoBehaviour {

    public Vector3 direction = new Vector3(1, 0, 0);
    public float speed = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var tag = collision.gameObject.name;
        if (tag != "Player") direction.x *= -1;
    }

    // Move it along its direction.
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
}
