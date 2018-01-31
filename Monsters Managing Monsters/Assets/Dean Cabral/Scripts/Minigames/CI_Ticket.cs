using UnityEngine;
using System.Collections;

public class CI_Ticket : MonoBehaviour
{
    public int expireTime;

    private void OnEnable()
    {
    }

    void Update()
    {
        
    }

    IEnumerator DestroyTicket()
    {
        yield return new WaitForSeconds(expireTime);
        Destroy(gameObject);
    }
}
