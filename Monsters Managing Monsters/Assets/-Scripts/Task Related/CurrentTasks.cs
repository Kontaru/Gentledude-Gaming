using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTasks : MonoBehaviour {

    public Task[] currentTask = new Task[5];
    //Do a thing which stores a queue of tasks
    //Do another thing which only displays the recent tasks
    //Do a final thing where if a task is complete, do a delay on the delivering the next task (deliver after x minutes: y seconds)

    bool BL_allQuestsComplete = true;

    public static CurrentTasks instance;

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
    }

    #endregion

    // Use this for initialization
    void Start() {
        for (int i = 0; i < currentTask.Length; i++)
        {
            currentTask[i] = GrabRandomQuest();
        }
        currentTask[0].BL_isObtainable = true;
    }

    // Update is called once per frame
    void Update() {

        currentTask[0].BL_isObtainable = true;

        if (currentTask[0] == null || currentTask[0].Quest_Finish == true)
        {
            currentTask[0].inActiveList = false;

            if (HasAllQuestsAcquired()) NewQuestFromQueue();
            else
                NewQuestFromPool();
        }
    }

    bool HasAllQuestsAcquired()
    {
        BL_allQuestsComplete = true;

        foreach (Task task in TaskManager.instance.Tasks)
        {
            if (task.inActiveList == false && !task.Quest_Finish)
                BL_allQuestsComplete = false;
        }

        return BL_allQuestsComplete;
    }

    void NewQuestFromQueue()
    {
        ReorganiseQuests();
    }

    void NewQuestFromPool()
    {
        currentTask[0] = GrabRandomQuest();
        ReorganiseQuests();
    }

    void ReorganiseQuests()
    {
        Task FirstQuest = currentTask[0];
        for(int i = 1; i < currentTask.Length; i++)
        {
            currentTask[i - 1] = currentTask[i];
        }
        currentTask[currentTask.Length - 1] = FirstQuest;
    }

    Task GrabRandomQuest()
    {

        Task vTask = TaskManager.instance.Tasks[0];
        vTask = TaskManager.instance.Tasks[Random.Range(0, TaskManager.instance.Tasks.Length)];
        if (vTask.inActiveList == false && vTask.Quest_Finish == false)
        {
            vTask.inActiveList = true;
            return vTask;
        }
        else
            return GrabRandomQuest();
    }
}
