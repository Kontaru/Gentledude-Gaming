using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadHandler : MonoBehaviour{

    public static SaveLoadHandler instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    public bool SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/game.sav", FileMode.Create);

        GameData gd = new GameData();
        gd.day = GameManager.instance.day;
        gd.score = GameManager.instance.score;

        bf.Serialize(stream, gd);
        stream.Close();

        Debug.Log("Game Saved");
        return true;
    }

    public bool LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/game.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/game.sav", FileMode.Open);

            GameData gd = (GameData)bf.Deserialize(stream);
            stream.Close();

            GameManager.instance.day = gd.day;
            GameManager.instance.score = gd.score;

            Debug.Log("Game Loaded");
            return true;
        }
        else return false;              
    }

    public bool DeleteData()
    {
        if (File.Exists(Application.persistentDataPath + "/game.sav"))
        {
            File.Delete(Application.persistentDataPath + "/game.sav");
            return true;
        }
        else return false;
    }
}

[Serializable]
public class GameData
{
    public int day;
    public int score;
}
