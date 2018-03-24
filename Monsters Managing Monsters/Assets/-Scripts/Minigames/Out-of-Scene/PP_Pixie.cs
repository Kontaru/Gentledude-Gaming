using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Pixie : MonoBehaviour {

    private float speed;
    private int randomFactor;
    private bool BL_escaping;
    private Vector3 _startPosition;
    private PP_PlayerBehaviour PB;
    private GameObject carriedGremlin;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PS_Base")
        {
            PB.DestroyGremlin();
            if (PB.pixieCount > 0) PB.pixieCount--;
            Escape();
        }
    }

    private void OnEnable()
    {
        PB = FindObjectOfType<PP_PlayerBehaviour>();
        carriedGremlin = transform.GetChild(0).gameObject;
        _startPosition = transform.position;
        randomFactor = Random.Range(1, 4);
        Destroy(gameObject, 8);
    }

    void Update()
    {
        MovePixie();
    }

    void MovePixie()
    {
        if (BL_escaping)
        {
            transform.position += Vector3.up * (speed + 2) * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.down * (speed + 2) * Time.deltaTime;
            transform.position = new Vector3(_startPosition.x + Mathf.Sin(Time.time) * randomFactor, transform.position.y, transform.position.z);
        }        
    }    

    private void Escape()
    {
        Destroy(GetComponent<Rigidbody2D>());
        BL_escaping = true;
        carriedGremlin.SetActive(true);
    }

    public void SpeedType(int _type)
    {
        speed = _type;
    }
}
