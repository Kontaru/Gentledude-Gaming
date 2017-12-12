using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myQuests : MonoBehaviour {

    [Header("Quest IDs")]
    public int[] QuestIDs;
    [Header("Tasks Assigned")]
    public Task[] Tasks;

    private void Start()
    {
        Tasks = new Task[QuestIDs.Length];

        for (int i = 0; i < QuestIDs.Length; i++)
        {
            foreach (Task task in TaskManager.instance.Tasks)
            {
                if (task.QuestID == QuestIDs[i])
                {
                    Tasks[i] = task;
                }
            }

            if (Tasks[i] == null)
            {
                Debug.Log("Task " + i + " not acquired");
            }
        }
    }
}
