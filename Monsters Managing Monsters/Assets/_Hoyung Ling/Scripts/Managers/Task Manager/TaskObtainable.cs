using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskObtainable : MonoBehaviour {



    public void MakeAvailable(int QuestID)
    {
        foreach (Task task in TaskManager.instance.Tasks)
        {
            if (task.QuestID == QuestID)
                task.isObtainable = true;
        }
    }
}
