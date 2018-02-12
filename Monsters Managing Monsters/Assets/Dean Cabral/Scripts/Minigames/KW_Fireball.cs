using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KW_Fireball : MonoBehaviour {

    private float speed;
    private bool leftSpawner;
    private Vector3 _startPosition;
    private float randomFactor;

    private void OnEnable()
    {
        StartCoroutine(DestroyFireball());
        randomFactor = Random.Range(0, 2);
    }

    void Update()
    {
        MoveFireball();
    }

    void MoveFireball()
    {
        if (leftSpawner) transform.position += Vector3.right * (speed + 2) * Time.deltaTime;
        else transform.position += Vector3.left * speed * Time.deltaTime;

        if (randomFactor == 1) transform.position += Vector3.up / 2 * speed * Time.deltaTime;
        else transform.position += Vector3.down / 2 * speed * Time.deltaTime;
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

    IEnumerator DestroyFireball()
    {
        yield return new WaitForSeconds(12);
        Destroy(this.gameObject);
    }
}
