using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndDaySummary : MonoBehaviour {

    public static EndDaySummary instance;

    private int tasksCount;
    private int dept1Motivation = 0;
    private int dept2Motivation = 0;
    private int dept3Motivation = 0;
    private int penalties = 0;
    private int penalty1 = 100;
    private int penalty2 = 100;
    private int penalty3 = 100;

    public Text tasksComplete, dept1Text, dept2Text, dept3Text, pen1Text, pen2Text, pen3Text, scoreText;

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

    private void Update()
    {
        tasksComplete.text = tasksCount.ToString();
        dept1Text.text = "+" + dept1Motivation;
        dept2Text.text = "+" + dept2Motivation;
        dept3Text.text = "+" + dept3Motivation;
        pen1Text.text = "-" + penalty1;
        pen2Text.text = "-" + penalty2;
        pen3Text.text = "-" + penalty3;
        penalties = penalty1 + penalty2 + penalty3;
        scoreText.text = dept1Motivation + dept2Motivation + dept3Motivation - penalties + "";
    }

    public void IncreaseTaskCount()
    {
        tasksCount++;
    }

    public void IncreaseMotivation(int amount, int dept)
    {
        switch (dept)
        {
            case 1:
                dept1Motivation += amount;
                break;
            case 2:
                dept2Motivation += amount;
                break;
            case 3:
                dept3Motivation += amount;
                break;
        }
    }

    public void DecreaseMotivation(int amount)
    {
        dept1Motivation -= amount;
        dept2Motivation -= amount;
        dept3Motivation -= amount;
    }
}
