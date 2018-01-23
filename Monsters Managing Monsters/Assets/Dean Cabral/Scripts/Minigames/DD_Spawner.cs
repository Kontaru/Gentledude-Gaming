using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DD_Spawner : MonoBehaviour {

    public GameObject car;
    private int previousValue = 0;
    private int speed;

    private void OnEnable()
    {
        StartCoroutine(RandomSpawn());
    }

    private void SpawnCar()
    {
        GameObject GO = Instantiate(car, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        GO.SendMessage("SpawnerType", this.transform.name);
        GO.SendMessage("SpeedType", GetRandomSpeed());
    }

    IEnumerator RandomSpawn()
    {
        yield return new WaitForSeconds(GetRandomSeconds());
        SpawnCar();
        StartCoroutine(RandomSpawn());
    }

    private int GetRandomSeconds()
    {
        int value = Random.Range(0, 2);

        while (value == previousValue)
        {
            value = Random.Range(0, 2);
        }
        previousValue = value;

        return value;
    }

    private int GetRandomSpeed()
    {
        int value = Random.Range(4, 8);
        return value;
    }
}
