using UnityEngine;
using System.Collections;

public class PC_PopUp : MonoBehaviour
{
    private PC_PlayerBehaviour PB;

    private void OnEnable()
    {
        PB = FindObjectOfType<PC_PlayerBehaviour>();
        Destroy(gameObject, 8);
    }

    public void ClosePopup()
    {
        PB.popupsClosed++;
        Destroy(gameObject);
    } 
}
