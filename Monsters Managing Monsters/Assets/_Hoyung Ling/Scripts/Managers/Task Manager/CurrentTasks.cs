using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTasks : MonoBehaviour {

    public static Task[] currentTask = new Task[5];
    //Do a thing which stores a queue of tasks
    //Do another thing which only displays the recent tasks
    //Do a final thing where if a task is complete, do a delay on the delivering the next task (deliver after x minutes: y seconds)

    bool BL_allQuestsComplete = true;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < currentTask.Length; i++)
        {
            currentTask[i] = GrabRandomQuest();
        }
    }
	
	// Update is called once per frame
	void Update () {

        BL_allQuestsComplete = true;

        foreach (Task task in TaskManager.instance.Tasks)
        {
            if (task.inActiveList == true || !task.Quest_Finish)
                BL_allQuestsComplete = false;
        }

        if (BL_allQuestsComplete) return;

        for (int i = 0; i < currentTask.Length; i++)
        {
            if (currentTask[i].Quest_Finish == true)
            {
                currentTask[i] = GrabRandomQuest();
            }
            else
            {
                currentTask[i].BL_isObtainable = true;
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
            return GrabRandomQuest();
    }
}
