using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float deathTimer;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, deathTimer);
    }
}
