using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScenarioBuilder : MonoBehaviour {

    public List<Task> PlayerQuests = new List<Task>();           //Generated - Display this as the top 5 possible quests
    public List<Task> DailyQuests;
    public bool trigger = false;
    List<Task> PreSortedDailyQuests;

    public int ApLim;
    TaskManager taskManager;

	// Use this for initialization
	void Start () {
        taskManager = TaskManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if (trigger)
        {
            trigger = false;
            GenerateDailyQuests(taskManager.Tasks);
        }
	}

    void GenerateDailyQuests(Task[] all)
    {
        //Sort based on max spendable AP
        int spendableAP = ApLim;
        int questLims = spendableAP + 5;
        List<Task> allquests = TaskManager.instance.Tasks.ToList();
        List<Task> dailyQuests = new List<Task>();

        for (int current = 0; current < allquests.Count; current++)
        {
            if (allquests[current].Quest_Finish)
                break;
            else
            {
                int tQuestLims = 0;
                if (tQuestLims == 0)
                    dailyQuests.Add(allquests[current]);
                else
                {
                    for (int index = 0; index < dailyQuests.Count; index++)
                    {
                        tQuestLims += dailyQuests[index].IN_actionPointWeight;
                    }
                    if (tQuestLims + allquests[current].IN_actionPointWeight >= questLims)
                        break;
                    else
                        dailyQuests.Add(allquests[current]);
                }
            }
        }

        PreSortedDailyQuests = dailyQuests;
        RankDailyQuests(PreSortedDailyQuests);
    }

    void RankDailyQuests(List<Task> preSorted)
    {
        List<Task> sorted = preSorted;

        sorted.Sort(delegate (Task x, Task y) { return x.IN_actionPointWeight.CompareTo(y.IN_actionPointWeight); });

        DailyQuests = sorted;
        GeneratePlayerPossibilities(DailyQuests);
    }

    void GeneratePlayerPossibilities(List<Task> dailyQuests)
    {
        List<Task> shuffle = dailyQuests;
        List<Task> tempList = new List<Task>(shuffle.Count);

        List<Task> playerQuests = new List<Task>();

        for (int current = 0; current < shuffle.Count; current++)
        {
            Task storedTask = shuffle[current];
            int searchedTask = Random.Range(current, shuffle.Count);
            shuffle[current] = shuffle[searchedTask];
            shuffle[searchedTask] = storedTask;
        }

        for (int current = 0; current < DailyQuests.Count || current < 5; current++)
        {
            playerQuests[current] = shuffle[current];
        }

        PlayerQuests = playerQuests;
    }
}
