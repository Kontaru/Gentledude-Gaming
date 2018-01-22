using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_Pixie : MonoBehaviour {

    private float speed;
    private Vector3 _startPosition;
    private PS_PlayerBehaviour PB;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PS_Base")
        {
            PB.DestroyGremlin();
        }

    }
    void Start()
    {
        PB = FindObjectOfType<PS_PlayerBehaviour>();
        _startPosition = transform.position;
        StartCoroutine(DestroyPixie());
    }
    void Update()
    {
        MovePixie();
    }

    void MovePixie()
    {
        transform.position += Vector3.down * (speed + 2) * Time.deltaTime;
        transform.position = new Vector3(_startPosition.x + Mathf.Sin(Time.time) * 2f, transform.position.y, transform.position.z);
    }    

    public void SpeedType(int _type)
    {
        speed = _type;
    }

    IEnumerator DestroyPixie()
    {
        yield return new WaitForSeconds(6);
        Destroy(this.gameObject);
    }
}
