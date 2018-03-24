using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SS_PlayerBehaviour : MonoBehaviour {

    public bool BL_GameComplete = false;
    public bool BL_MinigameFailed;
    public GameObject handsParent;
    public GameObject[] hands;
    public GameObject hand;
    public Transform[] points;
    public GameObject[] items;
    public GameObject winScreen;
    public GameObject failScreen;
    public Text timerText, handsSwattedCount;

    public int swatCount;
    private List<GameObject> activeHands;
    private int timer;
    private int previousValue;

    private void OnEnable()
    {
        AudioManager.instance.Play("BGM Minigame");
        AudioManager.instance.Stop("Dungeon Music");
        AudioManager.instance.Stop("Theme");

        BL_GameComplete = false;
        BL_MinigameFailed = false;
        winScreen.SetActive(false);
        failScreen.SetActive(false);

        activeHands = new List<GameObject>();
        timer = 30;
        swatCount = 0;
        StartCoroutine(CountdownTimer());
        StartCoroutine(RandomSpawn());
    }

    void Update()
    {
        UpdateUI();
        CheckFailure();
    }

    private void UpdateUI()
    {
        timerText.text = "Timer: " + timer.ToString() + " seconds";
        handsSwattedCount.text = "Hands Swatted: " + swatCount.ToString();
    }

    private void CheckFailure()
    {
        if (swatCount >= 25) WinScreen();

        if (BL_MinigameFailed)
        {
            AudioManager.instance.Stop("BGM Minigame");
            AudioManager.instance.Play("Dungeon Music");
            ClearHands();
            failScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void WinScreen()
    {
        AudioManager.instance.Stop("BGM Minigame");
        AudioManager.instance.Play("Dungeon Music");
        BL_GameComplete = true;
        BL_MinigameFailed = false;
        ClearHands();
        winScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ReturnToMenu()
    {
        GameManager.instance.LoadScene(0);
    }

    private void SpawnHand()
    {
        int index = GetRandomPointIndex();

        GameObject GO = Instantiate(hand, Vector3.zero, Quaternion.identity);
        GO.transform.SetParent(handsParent.transform, false);
        GO.transform.position = points[index].position;

        if (index == 0)
        {
            GO.GetComponent<SS_Hand>().direction = Vector3.right;
            GO.transform.rotation = Quaternion.Euler(-180, 0, -180);
        }
        else if (index == 4)
        {
            GO.GetComponent<SS_Hand>().direction = Vector3.left;
            GO.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            GO.GetComponent<SS_Hand>().direction = Vector3.down;
            GO.transform.rotation = Quaternion.Euler(0, 0, 60);
        }
            

        activeHands.Add(GO);
    }

    private int GetRandomPointIndex()
    {
        int value = Random.Range(0, 5);

        while (value == previousValue)
        {
            value = Random.Range(0, 5);
        }
        previousValue = value;

        return value;
    }

    private int GetRandomSeconds()
    {
        return Random.Range(1, 3);
    }

    private void ClearHands()
    {
        foreach (GameObject hand in activeHands)
        {
            Destroy(hand);
        }
    }

    IEnumerator RandomSpawn()
    {
        yield return new WaitForSeconds(GetRandomSeconds());
        SpawnHand();
        StartCoroutine(RandomSpawn());
    }

    IEnumerator CountdownTimer()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
        }

        BL_MinigameFailed = true;
    }
}
