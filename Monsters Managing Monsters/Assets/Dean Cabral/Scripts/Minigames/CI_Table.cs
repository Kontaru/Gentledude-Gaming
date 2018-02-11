using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CI_Table : MonoBehaviour
{
    public bool BL_tableCleared;
    public bool[] items;
    public Text tableText;
    private string ST_order;
    private CI_PlayerBehaviour PB;

    void OnEnable()
    {
        PB = FindObjectOfType<CI_PlayerBehaviour>();
        items = new bool[4];
        StartCoroutine(GetOrder());
    }

    void Update()
    {
        tableText.text = ST_order;
    }

    IEnumerator GetOrder()
    {
        yield return new WaitForSeconds(1);
        BL_tableCleared = false;

        for (int i = 0; i < 4; i++)
        {
            int rand = Random.Range(0, 2);
            items[i] = (rand == 1);

            if (rand == 1) ST_order += "Item " + (i + 1) + "\n";
        }

        StartCoroutine(ExpireTicket());
    }

    public void ClearItems(bool flag)
    {
        for (int i = 0; i < 4; i++)
        {
            items[i] = false;
        }
        ST_order = "";

        if (flag) StartCoroutine(GetOrder());
    }

    IEnumerator ExpireTicket()
    {
        yield return new WaitForSeconds(16);
        if (!BL_tableCleared)
        {
            ClearItems(false);
            PB.missedOrders++;
            yield return new WaitForSeconds(2);
            StartCoroutine(GetOrder());
        }       
    }
}
