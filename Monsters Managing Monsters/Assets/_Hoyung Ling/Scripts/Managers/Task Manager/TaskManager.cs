using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour {

    public Task[] Tasks;                //A collection of tasks

    public static TaskManager instance;

    #region Typical Singleton Format

    void Awake()
    {

        //Singleton stuff
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    void Update()
    {
        //For all our tasks, check if it's complete or not.
        foreach (Task task in Tasks)
            task.StepChecker();
    }
}
