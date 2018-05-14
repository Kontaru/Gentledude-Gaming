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
        anim.SetBool("open", true);
        //StartCoroutine(WatchDoor());
    }

    public void Close()
    {
        anim.SetBool("open", false);
        //StartCoroutine(WatchDoor());
    }

    IEnumerator WatchDoor()
    {
        CameraFollow.instance.otherLook = transform.gameObject;
        PC_Move.BL_canMove = false;
        yield return new WaitForSeconds(2f);
        CameraFollow.instance.otherLook = null;
        PC_Move.BL_canMove = true;
    }
}
