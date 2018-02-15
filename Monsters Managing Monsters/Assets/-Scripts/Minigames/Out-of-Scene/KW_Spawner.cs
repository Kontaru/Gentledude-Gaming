using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KW_Spawner : MonoBehaviour {

    public GameObject fireball;
    private int previousValue = 0;
    private int speed;

    private void OnEnable()
    {
        StartCoroutine(RandomSpawn());
    }

    private void SpawnFireball()
    {
        GameObject GO = Instantiate(fireball, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        GO.SendMessage("SpawnerType", this.transform.name);
        GO.SendMessage("SpeedType", GetRandomSpeed());
    }

    IEnumerator RandomSpawn()
    {
        yield return new WaitForSeconds(GetRandomSeconds());
        SpawnFireball();
        StartCoroutine(RandomSpawn());
    }

    private int GetRandomSeconds()
    {
        int value = Random.Range(0, 4);

        while (value == previousValue)
        {
            value = Random.Range(0, 4);
        }
        previousValue = value;

        return value;
    }

    private int GetRandomSpeed()
    {
        int value = Random.Range(2, 4);
        return value;
    }
}
