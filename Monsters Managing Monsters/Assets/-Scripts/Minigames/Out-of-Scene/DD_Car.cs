﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DD_Car : MonoBehaviour {

    private float speed;
    private bool leftSpawner;
    private Vector3 _startPosition;

    private void OnEnable()
    {
        StartCoroutine(DestroyCar());
    }

    void Update()
    {
        MoveCar();
    }

    void MoveCar()
    {
        if (leftSpawner) transform.position += Vector3.right * (speed + 2) * Time.deltaTime;
        else transform.position += Vector3.left * speed * Time.deltaTime;        
    }

    void SpawnerType(string _type)
    {
        if (_type == "LeftSpawner")
        {
            leftSpawner = true;
        }
        else
        {
            leftSpawner = false;
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void SpeedType(int _type)
    {
        speed = _type;
    }

    IEnumerator DestroyCar()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
