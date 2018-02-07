using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTasks : MonoBehaviour {

    public Task[] currentTask = new Task[5];
    //Do a thing which stores a queue of tasks
    //Do another thing which only displays the recent tasks
    //Do a final thing where if a task is complete, do a delay on the delivering the next task (deliver after x minutes: y seconds)

    bool allLimit = true;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < currentTask.Length; i++)
        {
            currentTask[i] = GrabRandomQuest();
        }
    }
	
	// Update is called once per frame
	void Update () {

        allLimit = true;

        foreach (Task task in TaskManager.instance.Tasks)
        {
            if (task.inActiveList == true || !task.QuestFinish)
                allLimit = false;
        }

        if (allLimit) return;

        for (int i = 0; i < currentTask.Length; i++)
        {
            if (currentTask[i].QuestFinish == true)
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
        if (vTask.inActiveList == false)
        {
            vTask.inActiveList = true;
            return vTask;
        }
        else
            return TaskManager.instance.Tasks[3];
    }
}
