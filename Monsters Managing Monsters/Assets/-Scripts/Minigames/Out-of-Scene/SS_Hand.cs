using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SS_Hand : MonoBehaviour {

    public Vector3 direction;

    private bool BL_escaping;
    private float speed;
    private Vector3 _startPosition;
    private GameObject carriedItem;
    private SS_PlayerBehaviour PB;

    private void OnEnable()
    {
        PB = FindObjectOfType<SS_PlayerBehaviour>();
        Destroy(gameObject, 8);
        StartCoroutine(StealItem());
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().color =  Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        carriedItem = transform.GetChild(0).gameObject;
    }

    public void HandSwatted()
    {
        PB.swatCount++;
        Escape(false);
    }

    void Update()
    {
        MoveHand();
    }

    void MoveHand()
    {
        if (BL_escaping)
        {
            transform.position += -direction * (speed + 4) * Time.deltaTime;
        }
        else
        {
            transform.position += direction * (speed + 2) * Time.deltaTime;
            if (direction == Vector3.right || direction == Vector3.left) transform.position += Vector3.down * (speed + 2) * Time.deltaTime;
        }
    }

    public void Escape(bool hasItem)
    {
        Destroy(GetComponent<Rigidbody2D>());
        BL_escaping = true;
        if (hasItem)
        {
            carriedItem.SetActive(true);
        }               
    }

    IEnumerator StealItem()
    {
        yield return new WaitForSeconds(2.5f);
        Escape(true);
    }
}
