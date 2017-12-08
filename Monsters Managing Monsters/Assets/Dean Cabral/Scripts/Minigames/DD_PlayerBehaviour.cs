using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DD_PlayerBehaviour : MonoBehaviour {

    public bool BL_MinigameFailed;
    public GameObject doughnuts;
    public Text timerText;
    public Text livesText;

    private Vector3 playerSpawn;
    private int lives;
    private int timer;
    private bool BL_DoughnutsVisible = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Doughnuts(Clone)")
        {
            Destroy(collision.gameObject);
            BL_DoughnutsVisible = false;
        }
        else RespawnPlayer();
    }

    // Use this for initialization
    void Start () {

        playerSpawn = transform.position;
        lives = 3;
        timer = 30;
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
        if (BL_MinigameFailed) Time.timeScale = 0;
    }

    private void RespawnPlayer()
    {
        if (lives > 0)
        {
            lives--;
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
}
