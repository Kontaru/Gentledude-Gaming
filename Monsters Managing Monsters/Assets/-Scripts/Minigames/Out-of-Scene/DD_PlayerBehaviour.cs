
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DD_PlayerBehaviour : MonoBehaviour {

    public bool BL_GameComplete = false;
    public bool BL_MinigameFailed;
    public GameObject doughnuts;
    public Transform doughnutSpawn;
    public GameObject winScreen;
    public GameObject failScreen;
    public Text timerText;
    public Text livesText;
    public Text collectedText;
    public Text invText;

    public static Vector3 playerSpawn;
    private int doughnutsCollected = 0;
    private int lives;
    private int timer;
    private string inventory;
    public bool BL_DoughnutsVisible = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Doughnuts(Clone)")
        {
            Destroy(collision.gameObject);
            inventory = "Doughnuts";
            BL_DoughnutsVisible = false;
        }
        else RespawnPlayer(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "DD_Office" && inventory == "Doughnuts")
        {            
            if (doughnutsCollected >= 2) WinScreen();
            doughnutsCollected++;
            RespawnPlayer(false);
        }
    }

    private void OnEnable ()
    {
        AudioManager.instance.Play("BGM Minigame");
        AudioManager.instance.Stop("Dungeon Music");
        AudioManager.instance.Stop("Theme");

        BL_GameComplete = false;
        BL_MinigameFailed = false;
        winScreen.SetActive(false);
        failScreen.SetActive(false);        

        playerSpawn = transform.position;
        lives = 3;
        timer = 30;
        doughnutsCollected = 0;
        inventory = "Empty";
        RespawnPlayer(true);

        StartCoroutine(CountdownTimer());
	}
	
	void Update () {

        UpdateUI();
        CheckMovement();
        CheckFailure();
	}

    private void UpdateUI()
    {
        timerText.text = "Timer: " + timer.ToString() + " seconds";
        livesText.text = "Lives: " + lives.ToString();
        collectedText.text = "Collected: " + doughnutsCollected.ToString() + "/3";
        invText.text = "Inventory: " + inventory;
    }

    private void CheckMovement()
    {
        if (BL_GameComplete) return;

        if (Input.GetKeyDown(KeyCode.W)) transform.position += Vector3.up * 1.5f;
        if (Input.GetKeyDown(KeyCode.S)) transform.position += Vector3.down * 1.5f;
        if (Input.GetKeyDown(KeyCode.A)) transform.position += Vector3.left;
        if (Input.GetKeyDown(KeyCode.D)) transform.position += Vector3.right;
    }

    private void CheckFailure()
    {
        if (BL_MinigameFailed)
        {
            AudioManager.instance.Stop("BGM Minigame");
            AudioManager.instance.Play("Dungeon Music");
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
        winScreen.SetActive(true);
        Time.timeScale = 0;
    }

    private void RespawnPlayer(bool killed)
    {
        if (lives > 0)
        {
            if (killed)
            {
                lives--;
                transform.position = playerSpawn;
            }            
            inventory = "Empty";
        }
        else
        {
            BL_MinigameFailed = true;
            BL_GameComplete = true;
        }

        transform.position = playerSpawn;

        if (!BL_DoughnutsVisible)
        {
            GameObject GO = Instantiate(doughnuts, new Vector3(-118, -4.5f, -180), doughnuts.transform.rotation);
            BL_DoughnutsVisible = !BL_DoughnutsVisible;
        }            
    }

    public void ReturnToMenu()
    {
        GameManager.instance.LoadScene(0);
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
