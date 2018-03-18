using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PC_PlayerBehaviour : MonoBehaviour {

    public bool BL_GameComplete = false;
    public bool BL_MinigameFailed;
    public GameObject popupParent;
    public GameObject[] popups;
    public Transform[] points;
    public GameObject winScreen;
    public GameObject failScreen;
    public Text timerText, popupsClosedCount;

    public int popupsClosed;
    private List<GameObject> activePopups;
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

        activePopups = new List<GameObject>();
        timer = 30;
        popupsClosed = 0;
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
        popupsClosedCount.text = "Popups Closed: " + popupsClosed.ToString();
    }

    private void CheckFailure()
    {
        if (popupsClosed >= 25) WinScreen();

        if (BL_MinigameFailed)
        {
            AudioManager.instance.Stop("BGM Minigame");
            AudioManager.instance.Play("Dungeon Music");
            ClearPopups();
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
        ClearPopups();
        winScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ReturnToMenu()
    {
        GameManager.instance.LoadScene(0);
    }

    private void SpawnAd()
    {
        Vector3 point = points[GetRandomPointIndex()].position;

        GameObject GO = Instantiate(popups[GetRandomAdIndex()], Vector3.zero, Quaternion.identity);
        GO.transform.SetParent(popupParent.transform, false);
        GO.transform.position = new Vector3(point.x, point.y, point.z);
        activePopups.Add(GO);
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

    private int GetRandomAdIndex()
    {
        int value = Random.Range(0, 3);
        return value;
    }

    private int GetRandomSeconds()
    {
        return Random.Range(0, 2);
    }

    private void ClearPopups()
    {
        foreach (GameObject popup in activePopups)
        {
            Destroy(popup);
        }
    }

    IEnumerator RandomSpawn()
    {
        yield return new WaitForSeconds(1);
        SpawnAd();
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
