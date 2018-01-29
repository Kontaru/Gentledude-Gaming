using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTasks : MonoBehaviour {

    public Task[] currentTask = new Task[5];
    //Do a thing which stores a queue of tasks
    //Do another thing which only displays the recent tasks
    //Do a final thing where if a task is complete, do a delay on the delivering the next task (deliver after x minutes: y seconds)
    
	// Use this for initialization
	void Start () {
        //currentTask[0] = TaskManager.instance.Tasks[0];
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < currentTask.Length; i++)
        {
            if (currentTask[i] == null)
            {
                currentTask[i] = GrabRandomQuest();
            }
            else if (currentTask[i].QuestComplete == true)
            {
                currentTask[i] = GrabRandomQuest();
            }
            else
            {
                currentTask[i].isObtainable = true;
            }
        }
	}

    Task GrabRandomQuest()
    {
        Task vTask = TaskManager.instance.Tasks[0];
        vTask = TaskManager.instance.Tasks[Random.Range(0, TaskManager.instance.Tasks.Length)];

        return vTask;
    }
}
