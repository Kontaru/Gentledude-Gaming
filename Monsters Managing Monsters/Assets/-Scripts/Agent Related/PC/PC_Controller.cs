using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Controller : Entity {

    //Bool
    public bool BL_IsMoving;
    public GameObject pointer;
    private GameObject target;

    //Controllers
    PC_Move CC_Move;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "AutoDoors") other.gameObject.GetComponent<AutoDoors>().OpenDoors(true);
        else if (other.gameObject.name == "TutorialTrigger") PDAHandler.instance.ShowTutorial();
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
        PointAtTarget();
        FindTarget();
    }

    private void FindTarget()
    {
        for (int i = 0; i < CurrentTasks.currentTask.Length; i++)
        {
            Task task = CurrentTasks.currentTask[i];

            if (task.BL_isObtainable && !task.BL_isAccepted || task.Quest_Complete)
            {
                target = task.GO_belongsTo.gameObject;
                break;
            }
            else if (task.BL_isAccepted)
            {
                target = task.Steps[task.step_tracker].requires.gameObject;
                break;
            }  
        }
    }

    private void PointAtTarget()
    {
        if (target != null)
        {
            pointer.SetActive(true);
            pointer.transform.LookAt(target.transform.position);
        }
        else
        {
            pointer.SetActive(false);
        }
        
    }
}
