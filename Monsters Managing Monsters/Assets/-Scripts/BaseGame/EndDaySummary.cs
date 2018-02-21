using System.Collections;
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
    private int IN_HR, IN_IT, IN_JN, IN_MK, IN_FN, IN_OVR, IN_SEC;
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

    public void CalculateScores()
    {
        int mHR = GameManager.instance.PL_HR;
        int mIT = GameManager.instance.PL_IT;
        int mJN = GameManager.instance.PL_Janitorial;
        int mMK = GameManager.instance.PL_Marketing;
        int mFN = GameManager.instance.PL_Finance;
        int mOVR = GameManager.instance.PL_Overseas;
        int mSEC = GameManager.instance.PL_Security;

        StopAllCoroutines();
        StartCoroutine(CountTo(mHR, IN_HR, HR));
        StartCoroutine(CountTo(mIT, IN_IT, IT));
        StartCoroutine(CountTo(mJN, IN_JN, JN));
        StartCoroutine(CountTo(mMK, IN_MK, MK));
        StartCoroutine(CountTo(mFN, IN_FN, FN));
        StartCoroutine(CountTo(mOVR, IN_OVR, OVR));
        StartCoroutine(CountTo(mSEC, IN_SEC, SEC));
    }

    IEnumerator ShowNotification()
    {
        notifAnimator.SetBool("BL_ShowNotification", true);
        yield return new WaitForSeconds(3);
        notifAnimator.SetBool("BL_ShowNotification", false);
    }

    IEnumerator CountTo(int target, int score, Text label)
    {
        int start = score;
        yield return new WaitForSeconds(1);
        for (float timer = 0; timer < 3; timer += Time.deltaTime)
        {
            float progress = timer / 3;
            score = (int)Mathf.Lerp(start, target, progress);
            label.text = "+" + score.ToString();
            yield return null;
        }
        score = target;
        label.text = "+" + score.ToString();
    }
}
