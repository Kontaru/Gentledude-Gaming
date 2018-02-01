
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DD_PlayerBehaviour : MonoBehaviour {

    public bool BL_MinigameFailed;
    public GameObject doughnuts;
    public GameObject winScreen;
    public GameObject failScreen;
    public Text timerText;
    public Text livesText;
    public Text invText;

    private Vector3 playerSpawn;
    private int lives;
    private int timer;
    private string inventory;
    private bool BL_DoughnutsVisible = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Doughnuts(Clone)")
        {
            Destroy(collision.gameObject);
            inventory = "Doughnuts";
            BL_DoughnutsVisible = false;
        }
        else RespawnPlayer();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "DD_Office" && inventory == "Doughnuts")
        {
            WinScreen();
        }
    }

    // Use this for initialization
    private void OnEnable ()
    {
        playerSpawn = transform.position;
        lives = 3;
        timer = 30;
        inventory = "Empty";
        StartCoroutine(CountdownTimer());
	}
	
	// Update is called once per frame
	void Update () {

        UpdateUI();
        CheckMovement();
        CheckFailure();
	}

    private void UpdateUI()
    {
        timerText.text = "Timer: " + timer.ToString() + " seconds";
        livesText.text = "Lives: " + lives.ToString();
        invText.text = "Inventory: " + inventory;
    }

    private void CheckMovement()
    {
        if (Input.GetKeyDown(KeyCode.W)) transform.position += Vector3.up * 1.5f;
        if (Input.GetKeyDown(KeyCode.S)) transform.position += Vector3.down * 1.5f;
        if (Input.GetKeyDown(KeyCode.A)) transform.position += Vector3.left;
        if (Input.GetKeyDown(KeyCode.D)) transform.position += Vector3.right;
    }

    private void CheckFailure()
    {
        if (BL_MinigameFailed)
        {
            StartCoroutine(ShowScreen(failScreen));
        }
    }

    private void WinScreen()
    {
        StartCoroutine(ShowScreen(winScreen));
    }

    private void RespawnPlayer()
    {
        if (lives > 0)
        {
            lives--;
            inventory = "Empty";
            transform.position = playerSpawn;
        }
        else
        {
            BL_MinigameFailed = true;
        }

        if (!BL_DoughnutsVisible)
        {
            Instantiate(doughnuts, doughnuts.transform.position, doughnuts.transform.rotation);
            BL_DoughnutsVisible = !BL_DoughnutsVisible;
        }            
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

    IEnumerator ShowScreen(GameObject screen)
    {
        float lerpTime = 0;
        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * 3;
            screen.GetComponent<RectTransform>().localPosition = Vector3.Lerp(new Vector3(0, -345, 0), Vector3.zero, lerpTime);

            yield return null;
        }
        Time.timeScale = 0;
    }
}
