using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OE_PlayerBehaviour : MonoBehaviour
{

    public bool BL_GameComplete = false;
    public bool BL_MinigameFailed;

    public GameObject workersParent;
    public GameObject worker;
    public GameObject[] workers;

    public GameObject spawnPoint;
    public Transform[] waitPoints;
    private bool[] pointInUse = new bool[2];

    public GameObject winScreen;
    public GameObject failScreen;

    public Text timerText, escapedText;

    private List<GameObject> waitLine;

    public int escapedCount;    
    private int timer;
    private bool BL_commandGO;

    private void OnEnable()
    {
        AudioManager.instance.Play("BGM Minigame");
        AudioManager.instance.Stop("Dungeon Music");
        AudioManager.instance.Stop("Theme");

        BL_GameComplete = false;
        BL_MinigameFailed = false;
        winScreen.SetActive(false);
        failScreen.SetActive(false);

        waitLine = new List<GameObject>();

        timer = 0;
        escapedCount = 0;
        ClearWorkers();
        StartCoroutine(CountTimer());
        StartCoroutine(Spawn(true));
    }

    void Update()
    {
        CheckInput();
        UpdateUI();
        CheckFailure();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BL_commandGO = !BL_commandGO;

            if (BL_commandGO)
            {
                // PC Sprite GO
                StartCoroutine(LoopGo());
            }
            else
            {
                // PC Sprite STOP
                WorkersStop();
            }
        }        
    }

    private void UpdateUI()
    {
        timerText.text = "Timer: " + timer.ToString() + " seconds";
        escapedText.text = "Workers Escaped: " + escapedCount.ToString() + "/15";
    }

    private void CheckFailure()
    {
        if (escapedCount >= 15) WinScreen();

        if (BL_MinigameFailed)
        {
            AudioManager.instance.Stop("BGM Minigame");
            AudioManager.instance.Play("Dungeon Music");

            BL_commandGO = false;
            ClearWorkers();

            BL_GameComplete = true;
            failScreen.SetActive(true);
            
            Time.timeScale = 0;
        }
    }

    private void WinScreen()
    {
        AudioManager.instance.Stop("BGM Minigame");
        AudioManager.instance.Play("Dungeon Music");

        BL_MinigameFailed = false;
        BL_GameComplete = true;

        BL_commandGO = false;
        ClearWorkers();
        winScreen.SetActive(true);

        Time.timeScale = 0;
    }

    public void ReturnToMenu()
    {
        GameManager.instance.LoadScene(0);
    }

    private void SpawnWorker(int waitPoint)
    {
        GameObject GO = Instantiate(worker, Vector3.zero, Quaternion.identity);
        Vector3 spawn = spawnPoint.transform.position;
        GO.transform.SetParent(workersParent.transform, false);
        GO.transform.position = new Vector3(spawn.x, spawn.y, spawn.z);

        MoveWorkerTo(waitPoint, GO);       

        waitLine.Add(GO);
    }

    private void MoveWorkerTo(int point, GameObject GO)
    {
        OE_Worker mWorker = GO.GetComponent<OE_Worker>();

        if (point == 0) StartCoroutine(mWorker.MoveToPoint(waitPoints[0].position));
        else if (point == 1) StartCoroutine(mWorker.MoveToPoint(waitPoints[1].position));
    }

    private int GetRandomIndex()
    {
        int value = Random.Range(0, 3);
        return value;
    }

    private int GetRandomSeconds()
    {
        return Random.Range(0, 2);
    }

    private void ClearWorkers()
    {
        foreach (GameObject worker in waitLine)
        {
            Destroy(worker);
        }
    }

    private void WorkersGo()
    {
        if (pointInUse[1])
        {
            if (waitLine[0] == null) return;
            waitLine[0].GetComponent<OE_Worker>().Dash();
            waitLine.RemoveAt(0);
            pointInUse[1] = false;
        }

        if (pointInUse[0] && !pointInUse[1])
        {
            MoveWorkerTo(1, waitLine[0]);
            pointInUse[0] = false;
            pointInUse[1] = true;
        }
    }

    private void WorkersStop()
    {
        StopCoroutine(Spawn(false));
        StartCoroutine(Spawn(true));

        foreach (GameObject worker in waitLine)
        {
            worker.GetComponent<OE_Worker>().BL_canMove = false;
        }
    }

    IEnumerator LoopGo()
    {
        while (BL_commandGO)
        {
            yield return new WaitForSeconds(1);
            WorkersGo();
        }
    }

    IEnumerator Spawn(bool firstFlag)
    {
        if (firstFlag)
        {
            if (!pointInUse[1])
            {
                SpawnWorker(1);
                pointInUse[1] = true;
            }
        }

        if (!pointInUse[0])
        {
            SpawnWorker(0);
            pointInUse[0] = true;
        }
        yield return null;
        StartCoroutine(Spawn(false));
    }

    IEnumerator CountTimer()
    {
        while (timer < 60)
        {
            yield return new WaitForSeconds(1);
            timer++;
        }
    }
}
