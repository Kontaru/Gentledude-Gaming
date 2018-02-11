using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PP_PlayerBehaviour : MonoBehaviour
{
    public bool BL_MinigameFailed;
    public GameObject winScreen;
    public GameObject failScreen;
    public List<GameObject> gremlins;
    public Text pixieText;
    public Text gremlinText;

    private Vector3 playerSpawn;
    public int pixieCount;
    public int gremlinCount;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pixie(Clone)" && Input.GetKey(KeyCode.W))
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

    // Use this for initialization
    private void OnEnable()
    {
        pixieCount = 30;
        gremlinCount = 10;
        playerSpawn = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        CheckMovement();
        CheckFailure();
    }

    private void UpdateUI()
    {
        pixieText.text = pixieCount.ToString() + " Pixies Remaining";
        gremlinText.text = gremlinCount.ToString() + " Alive";
    }

    private void CheckMovement()
    {
        if (Input.GetKey(KeyCode.A)) transform.position += Vector3.left * 15 * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) transform.position += Vector3.right * 15 * Time.deltaTime;
    }

    private void CheckFailure()
    {
        if (gremlinCount <= 0 && !BL_MinigameFailed)
        {
            StartCoroutine(ShowScreen(failScreen));
            BL_MinigameFailed = true;
        }
        else if (pixieCount <= 0)
        {
            WinScreen();
        }
    }

    private void WinScreen()
    {
        StartCoroutine(ShowScreen(winScreen));
    }

    public void DestroyGremlin()
    {
        for (int i = 0; i < gremlins.Count; i++)
        {
            if (gremlins[i] != null)
            {
                Destroy(gremlins[i]);
                gremlins.RemoveAt(i);
                break;
            }
        }

        if (gremlinCount > 0) gremlinCount--;
    }

    public void ReturnToMenu()
    {
        GameManager.instance.LoadScene(0);
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
