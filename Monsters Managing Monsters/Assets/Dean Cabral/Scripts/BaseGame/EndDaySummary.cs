using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndDaySummary : MonoBehaviour {

    public static EndDaySummary instance;

    private int tasksCount;
    private int dept1Motivation;
    private int dept2Motivation;
    private int dept3Motivation;
    private int penalties;
    private int penalty1;
    private int penalty2;
    private int penalty3;

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

        if (Input.GetKeyDown(KeyCode.X)) IncreaseMotivation(100, 1);
        if (Input.GetKeyDown(KeyCode.C)) IncreaseMotivation(100, 2);
        if (Input.GetKeyDown(KeyCode.V)) IncreaseMotivation(100, 3);
    }

    public void IncreaseMotivation(int amount, int dept)
    {
        tasksCount++;
        if (dept == 1) dept1Motivation += amount;
        else if (dept == 2) dept2Motivation += amount;
        else if (dept == 3) dept3Motivation += amount;
    }

    public void DecreaseMotivation(int amount)
    {
        dept1Motivation -= amount;
        dept2Motivation -= amount;
        dept3Motivation -= amount;
    }
}
