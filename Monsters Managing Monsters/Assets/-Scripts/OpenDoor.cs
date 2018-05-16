using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    public static OpenDoor instance;
    public bool QuestEnded = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    Animator anim;

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Entity>().EntityType == Entity.Entities.Player && QuestEnded)
        {
            Close();
            QuestEnded = false;
        }
    }

    // Use this for initialization
    void Start () {
        anim = transform.GetChild(0).GetComponent<Animator>();
	}
	
	public void Open()
    {        
        StartCoroutine(WatchDoor(true));
    }

    public void Close()
    {        
        StartCoroutine(WatchDoor(false));
    }

    IEnumerator WatchDoor(bool state)
    {
        PC_Move.BL_canMove = false;
        yield return new WaitForSeconds(1);
        anim.SetBool("open", state);
        CameraFollow.instance.otherLook = transform.gameObject;

        yield return new WaitForSeconds(2f);
        CameraFollow.instance.otherLook = null;
        PC_Move.BL_canMove = true;
    }
}
