using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KW_PlayerBehaviour : MonoBehaviour {

    public bool BL_MinigameFailed;
    public GameObject winScreen;
    public GameObject failScreen;
    public Text timerText;
    public Text livesText;

    private Vector3 playerSpawn;
    private int lives;
    private int timer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RespawnPlayer();
    }
    private void OnEnable()
    {
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
        if (Input.GetKey(KeyCode.W)) transform.position += Vector3.up * 8 * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) transform.position += Vector3.down * 8 * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) transform.position += Vector3.left * 8 * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) transform.position += Vector3.right * 8 * Time.deltaTime;
    }

    private void CheckFailure()
    {
        if (BL_MinigameFailed)
        {
            StartCoroutine(ShowScreen(failScreen));
            BL_MinigameFailed = !BL_MinigameFailed;
        }
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
