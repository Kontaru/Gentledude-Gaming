using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PP_PlayerBehaviour : MonoBehaviour
{
    public bool BL_GameComplete = false;
    public bool BL_MinigameFailed;
    public GameObject winScreen;
    public GameObject failScreen;
    public List<GameObject> gremlins;
    public Text pixieText;
    public Text gremlinText;

    private PP_PixieSpawner PS;
    private Vector3 playerSpawn;
    public int pixieCount;
    public int gremlinCount;

    public bool BL_Swing = false;

    SpriteRenderer playerBroom;
    public Sprite BroomDown;
    public Sprite BroomUp;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pixie(Clone)" && BL_Swing)
        {
            int rand = Random.Range(1, 3);
            Rigidbody2D RB = collision.gameObject.GetComponent<Rigidbody2D>();            

            if (RB != null)
            {
                collision.gameObject.GetComponent<PP_Pixie>().enabled = false;

                if (rand == 1) RB.AddForce(Vector2.right * 600f);
                else RB.AddForce(Vector2.left * 600f);

                RB.AddForce(Vector2.up * 400f);
                if (pixieCount > 0) pixieCount--;
            }            
        }
    }

    private void OnEnable()
    {
        playerBroom = GetComponent<SpriteRenderer>();
        PS = FindObjectOfType<PP_PixieSpawner>();
        AudioManager.instance.Play("BGM Minigame");
        AudioManager.instance.Stop("Theme");
        AudioManager.instance.Stop("Dungeon Music");

        BL_GameComplete = false;
        BL_MinigameFailed = false;
        winScreen.SetActive(false);
        failScreen.SetActive(false);

        playerSpawn = transform.position;
        pixieCount = 30;
        gremlinCount = 10;
        SpawnGremlins();
        StartCoroutine(PS.RandomSpawn(pixieCount));
    }

    void Update()
    {
        UpdateUI();
        CheckMovement();
        CheckFailure();
    }

    IEnumerator BroomAnimator()
    {
        BL_Swing = true;
        playerBroom.sprite = BroomUp;
        yield return new WaitForSeconds(0.5f);
        BL_Swing = false;
        playerBroom.sprite = BroomDown;
    }

    private void UpdateUI()
    {
        pixieText.text = pixieCount.ToString() + " Pixies Remaining";
        gremlinText.text = gremlinCount.ToString() + " Alive";
    }

    private void CheckMovement()
    {
        if (BL_GameComplete) return;

        if (Input.GetKey(KeyCode.A)) transform.position += Vector3.left * 15 * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) transform.position += Vector3.right * 15 * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(BroomAnimator());
    }

    private void CheckFailure()
    {
        if (gremlinCount <= 0 && !BL_MinigameFailed)
        {
            AudioManager.instance.Stop("BGM Minigame");
            AudioManager.instance.Play("Dungeon Music");
            BL_MinigameFailed = true;
            BL_GameComplete = true;
            failScreen.SetActive(true);
            Time.timeScale = 0;
        }
        else if (pixieCount <= 0)
        {
            WinScreen();
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

    private void SpawnGremlins()
    {
        foreach (GameObject gremlin in gremlins)
        {
            gremlin.SetActive(false);
        }

        for (int i = 0; i < gremlins.Count; i++)
        {
            gremlins[i].SetActive(true);
        }
    }

    public void DestroyGremlin()
    {
        for (int i = 0; i < gremlins.Count; i++)
        {
            if (gremlins[i].activeSelf)
            {
                gremlins[i].SetActive(false);
                break;
            }
        }

        if (gremlinCount > 0) gremlinCount--;
    }

    public void ReturnToMenu()
    {
        GameManager.instance.LoadScene(0);
    }

}
