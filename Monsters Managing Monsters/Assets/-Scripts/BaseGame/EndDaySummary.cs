﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndDaySummary : MonoBehaviour {

    public static EndDaySummary instance;

    public GameObject notification;
    public Text notifTitleText;
    public Text notifText;

    private Animator notifAnimator;

    public int tasksCount;
    private int penalties;
    private int penalty1 = 50;
    private int penalty2 = 50;
    private int penalty3 = 100;

    public Text tasksComplete, HR, IT, JN, MK, FN, OVR, SEC, pen1Text, pen2Text, pen3Text, scoreText;

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

        HR.text = "+" + GameManager.instance.PL_HR;
        IT.text = "+" + GameManager.instance.PL_IT;
        JN.text = "+" + GameManager.instance.PL_Janitorial;
        MK.text = "+" + GameManager.instance.PL_Marketing;
        FN.text = "+" + GameManager.instance.PL_Finance;
        OVR.text = "+" + GameManager.instance.PL_Overseas;
        SEC.text = "+" + GameManager.instance.PL_Security;

        pen1Text.text = "-" + penalty1;
        pen2Text.text = "-" + penalty2;
        pen3Text.text = "-" + penalty3;

        int morale = GameManager.instance.PL_HR +
            GameManager.instance.PL_IT +
            GameManager.instance.PL_Janitorial +
            GameManager.instance.PL_Marketing +
            GameManager.instance.PL_Finance +
            GameManager.instance.PL_Overseas +
            GameManager.instance.PL_Security;

        penalties = penalty1 + penalty2 + penalty3;

        scoreText.text = morale - penalties + "";
    }

    public void DecreaseMotivation(int amount)
    {
        GameManager.instance.PL_HR -= amount;
        GameManager.instance.PL_HR -= amount;
        GameManager.instance.PL_Marketing -= amount;
    }

    void Start()
    {
        notifAnimator = notification.GetComponentInChildren<Animator>();
    }

    public void QuestGained(string questName)
    {
        notifTitleText.text = "New Quest!";
        notifText.color = Color.white;
        notifText.text = questName;
        StartCoroutine(ShowNotification());

    }
    public void QuestCompleted(string questName, bool failed)
    {
        if (failed)
        {
            notifTitleText.text = "Quest Failed!";
            notifText.color = Color.red;
        }
        else
        {
            notifTitleText.text = "Quest Complete!";
            notifText.color = Color.green;
        }
        
        notifText.text = questName;
        StartCoroutine(ShowNotification());
    }

    IEnumerator ShowNotification()
    {
        notifAnimator.SetBool("BL_ShowNotification", true);
        yield return new WaitForSeconds(4);
        notifAnimator.SetBool("BL_ShowNotification", false);
    }
}
