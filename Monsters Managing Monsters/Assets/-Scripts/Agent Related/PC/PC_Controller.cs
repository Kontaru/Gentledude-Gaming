using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Controller : Entity {

    //Bool
    public bool BL_IsMoving;

    //Controllers
    PC_Move CC_Move;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AutoDoors") other.gameObject.GetComponent<AutoDoors>().OpenDoors(true);
        else if (other.gameObject.name == "TutorialTrigger") StartCoroutine(TutorialSequence.instance.HRHeadIntroduction());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "AutoDoors") other.gameObject.GetComponent<AutoDoors>().OpenDoors(false);
    }

    // Use this for initialization
    void Start () {
        GameManager.instance.Player = gameObject;
        AudioManager.instance.Stop("Theme");
        AudioManager.instance.Play("Dungeon Music");
        CC_Move = GetComponent<PC_Move>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.PixelMode) return;

        BL_IsMoving = CC_Move.BL_isMoving;
    }
}
