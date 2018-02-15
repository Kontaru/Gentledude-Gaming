using UnityEngine;
using System.Collections;

public class CI_Station : MonoBehaviour
{
    public bool BL_Damaged;
    private int previousValue;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(RandomDamage());
    }

    // Update is called once per frame
    void Update()
    {
        if (BL_Damaged) gameObject.GetComponent<Renderer>().material.color = Color.black;
        else gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    IEnumerator RandomDamage()
    {
        yield return new WaitForSeconds(GetRandomSeconds());
        BL_Damaged = true;
        StartCoroutine(RandomDamage());
    }

    private int GetRandomSeconds()
    {
        int value = Random.Range(15, 30);

        while (value == previousValue)
        {
            value = Random.Range(15, 30);
        }
        previousValue = value;

        return value;
    }
}
