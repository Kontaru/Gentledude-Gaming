using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PS_PlayerBehaviour : MonoBehaviour
{
    public bool BL_MinigameFailed;
    public GameObject winScreen;
    public GameObject failScreen;
    public GameObject[] gremlins;
    public Text pixieText;
    public Text gremlinText;

    private Vector3 playerSpawn;
    public int pixieCount;
    public int gremlinCount;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pixie(Clone)")
        {
            int rand = Random.Range(1, 3);

            collision.gameObject.GetComponent<PS_Pixie>().enabled = false;

            if (rand == 1) collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 300f);
            else collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 300f);

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 400f);

        }
    }

    // Use this for initialization
    void Awake()
    {
        pixieCount = 30;
        gremlinCount = 6;
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
        for (int i = 0; i < gremlins.Length; i++)
        {
            if (gremlins[i] != null)
            {
                Destroy(gremlins[i]);
                break;
            }
        }

        gremlinCount--;
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
