using UnityEngine;
using System.Collections;

public class OE_Worker : MonoBehaviour
{
    public float speed = 5f;
    public bool BL_canMove;

    private bool isMoving = false;
    private OE_PlayerBehaviour PB;

    private void OnEnable()
    {
        PB = FindObjectOfType<OE_PlayerBehaviour>();
    }

    void Start()
    {
        BL_canMove = false;     
    }

    void Update()
    {
        if (BL_canMove)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }        
    }

    public void Dash()
    {
        BL_canMove = true;
        StartCoroutine(Escape());
    }

    public IEnumerator MoveToPoint(Vector3 point)
    {
        if (isMoving) yield break;
        isMoving = true;

        float counter = 0;

        Vector3 startPos = transform.position;

        while (Vector2.Distance(transform.position, point) > 0.1f)
        {
            counter += Time.deltaTime * 5;
            transform.position = Vector3.MoveTowards(startPos, point, counter / 1);
            yield return null;
        }

        transform.position = point;
        isMoving = false;
    }

    IEnumerator Escape()
    {
        yield return new WaitForSeconds(4);
        PB.escapedCount++;
        Destroy(gameObject);
    }
}
