using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SS_PlayerBehaviour : MonoBehaviour {

    public bool BL_GameComplete = false;
    public bool BL_MinigameFailed;

    public Camera cam;
    public GameObject handsParent;
    public GameObject[] hands;
    public GameObject hand;
    public Transform[] points;
    public GameObject[] items;
    public GameObject winScreen;
    public GameObject failScreen;
    public Text timerText, itemsCountText;

    public int swatCount;
    private int itemsCount;
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
        itemsCount = items.Length;
        swatCount = 0;
        ResetItems();
        StartCoroutine(CountdownTimer());
        StartCoroutine(RandomSpawn());
    }

    void Update()
    {
        CheckInput();
        UpdateUI();
        CheckFailure();        
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit) hit.collider.gameObject.GetComponent<SS_Hand>().HandSwatted();
        }
    }

    private void UpdateUI()
    {
        timerText.text = "Timer: " + timer.ToString() + " seconds";
        itemsCountText.text = "Items Remaining: " + itemsCount.ToString();
    }

    private void CheckFailure()
    {
        if (itemsCount <= 0) BL_MinigameFailed = true;

        if (BL_MinigameFailed)
        {
            AudioManager.instance.Stop("BGM Minigame");
            AudioManager.instance.Play("Dungeon Music");
            ClearHands();
            BL_MinigameFailed = true;
            BL_GameComplete = true;
            failScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void WinScreen()
    {
        AudioManager.instance.Stop("BGM Minigame");
        AudioManager.instance.Play("Dungeon Music");
        BL_GameComplete = true;
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
        return Random.Range(0, 3);
    }

    private void ClearHands()
    {
        foreach (GameObject hand in activeHands)
        {
            Destroy(hand);
        }
    }

    public void ClearItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].activeSelf)
            {
                items[i].SetActive(false);
                break;
            }
        }

        itemsCount--;
    }

    private void ResetItems()
    {
        foreach (GameObject item in items)
        {
            item.SetActive(true);
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

        WinScreen();
    }
}
