using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_PixieSpawner : MonoBehaviour {

    public GameObject pixie;
    public GameObject[] spawns;

    private int previousValue = 0;

    private void SpawnPixie()
    {
        GameObject spawn = spawns[GetRandomIndex()];
        GameObject GO = Instantiate(pixie, new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z), Quaternion.identity);
        GO.SendMessage("SpeedType", GetRandomSpeed());
    }

    private int GetRandomIndex()
    {
        int index = Random.Range(0, 4);
        return index;
    }

    private int GetRandomSpeed()
    {
        int value = Random.Range(1, 3);
        return value;
    }

    public IEnumerator RandomSpawn(int count)
    {
        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            SpawnPixie();
        }      
    }    
}
