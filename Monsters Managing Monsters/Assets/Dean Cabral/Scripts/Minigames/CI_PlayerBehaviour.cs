using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CI_PlayerBehaviour : MonoBehaviour
{
    public bool BL_MinigameFailed;
    public GameObject winScreen;
    public GameObject failScreen;
    public Text ordersText, missedText, repairText, invText;
    public int remainingOrders, missedOrders;
    private string inventory;
    public bool item1, item2, item3, item4;
    public bool repair1, repair2, repair3, repair4;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    // Use this for initialization
    private void OnEnable()
    {
        BL_MinigameFailed = false;
        winScreen.GetComponent<RectTransform>().localPosition = new Vector3(0, -345, 0);
        failScreen.GetComponent<RectTransform>().localPosition = new Vector3(0, -345, 0);
        remainingOrders = 5;
        missedOrders = 0;
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
        ordersText.text = remainingOrders + " Orders Remaining";
        missedText.text = missedOrders + " Missed Order(s)";

        if (repair1) repairText.text = "Repair Kit: 1";
        else if (repair2) repairText.text = "Repair Kit: 2";
        else if (repair3) repairText.text = "Repair Kit: 3";
        else if (repair4) repairText.text = "Repair Kit: 4";
        else if (!repair1 && !repair2 && !repair3 && !repair4) repairText.text = "Repair Kit: None";

        invText.text = "Inventory:" + inventory;
    }

    private void CheckMovement()
    {
        if (Input.GetKey(KeyCode.W)) transform.position += Vector3.up * 8 * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) transform.position += Vector3.down * 8 * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) transform.position += Vector3.left * 8 * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) transform.position += Vector3.right * 8 * Time.deltaTime;
    }

    private void HandleCollision(Collision2D coll)
    {
        if (coll.gameObject.name == "Station1")
        {
            CI_Station station = coll.gameObject.GetComponent<CI_Station>();
            if (station.BL_Damaged && repair1)
            {
                station.BL_Damaged = false;
                ResetRepairs();
            }
            else
            {
                if (station.BL_Damaged) return;
                if (!item1) inventory += "\nItem 1";
                item1 = true;
            }
        }
        else if (coll.gameObject.name == "Station2")
        {
            CI_Station station = coll.gameObject.GetComponent<CI_Station>();
            if (station.BL_Damaged && repair2)
            {
                station.BL_Damaged = false;
                ResetRepairs();
            }
            else
            {
                if (station.BL_Damaged) return;
                if (!item2) inventory += "\nItem 2";
                item2 = true;
            }
        }
        else if (coll.gameObject.name == "Station3")
        {
            CI_Station station = coll.gameObject.GetComponent<CI_Station>();
            if (station.BL_Damaged && repair3)
            {
                station.BL_Damaged = false;
                ResetRepairs();
            }
            else
            {
                if (station.BL_Damaged) return;
                if (!item3) inventory += "\nItem 3";
                item3 = true;
            }
        }
        else if (coll.gameObject.name == "Station4")
        {
            CI_Station station = coll.gameObject.GetComponent<CI_Station>();
            if (station.BL_Damaged && repair4)
            {
                station.BL_Damaged = false;
                ResetRepairs();
            }
            else
            {
                if (station.BL_Damaged) return;
                if (!item4) inventory += "\nItem 4";
                item4 = true;
            }
        }
        else if (coll.gameObject.name == "Repair1")
        {
            ResetRepairs();
            repair1 = true;
        }
        else if (coll.gameObject.name == "Repair2")
        {
            ResetRepairs();
            repair2 = true;
        }
        else if (coll.gameObject.name == "Repair3")
        {
            ResetRepairs();
            repair3 = true;
        }
        else if (coll.gameObject.name == "Repair4")
        {
            ResetRepairs();
            repair4 = true;
        }
        else if (coll.gameObject.name == "CI_Table1")
        {
            CI_Table table = coll.gameObject.GetComponent<CI_Table>();
            bool[] array = table.items;
            if (CheckItems(array))
            {
                table.ClearItems(true);
                table.BL_tableCleared = true;
                remainingOrders--;
            }
            ResetItems();
        }
        else if (coll.gameObject.name == "CI_Table2")
        {
            CI_Table table = coll.gameObject.GetComponent<CI_Table>();
            bool[] array = table.items;
            if (CheckItems(array))
            {
                table.ClearItems(true);
                table.BL_tableCleared = true;
                remainingOrders--;
            }
            ResetItems();
        }
        else if (coll.gameObject.name == "CI_Table3")
        {
            CI_Table table = coll.gameObject.GetComponent<CI_Table>();
            bool[] array = table.items;
            if (CheckItems(array))
            {
                table.ClearItems(true);
                table.BL_tableCleared = true;
                remainingOrders--;
            }
            ResetItems();
        }
    }

    private bool CheckItems(bool[] items)
    {
        if (items[0] == item1 && items[1] == item2 && items[2] == item3 && items[3] == item4) return true;
        else return false;
    }

    private void ResetItems()
    {
        item1 = false;
        item2 = false;
        item3 = false;
        item4 = false;
        inventory = "";
    }

    private void ResetRepairs()
    {
        repair1 = false;
        repair2 = false;
        repair3 = false;
        repair4 = false;
    }

    private void CheckFailure()
    {
        if (missedOrders >= 8) BL_MinigameFailed = true;
        if (remainingOrders <= 0) WinScreen();

        if (BL_MinigameFailed)
        {
            StartCoroutine(ShowScreen(failScreen));
        }
    }

    private void WinScreen()
    {
        StartCoroutine(ShowScreen(winScreen));
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
