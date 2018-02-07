using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero
{
    public string name;         //Hero name
    public GameObject hero;     //Hero prefab
    public int spawnIndex;      //Spawn index
    public int count = 1;       //Number of heroes

}
