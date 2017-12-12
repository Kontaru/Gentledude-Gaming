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
        currentTask[0] = TaskManager.instance.Tasks[0];
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < currentTask.Length; i++)
        {
            if (currentTask[i] == null)
            {
                GrabQuest(currentTask[i]);
            }
            else if (currentTask[i].QuestComplete == true)
            {
                GrabQuest(currentTask[i]);
            }
            else
            {
                currentTask[i].isObtainable = true;
            }
        }
	}

    void GrabQuest(Task current)
    {
        for (int i = 0; i < TaskManager.instance.Tasks.Length; i++)
        {
            if(TaskManager.instance.Tasks[i].QuestComplete)
            {
                break;
            }else
            {
                current = TaskManager.instance.Tasks[i];
            }
        }
    }
}
