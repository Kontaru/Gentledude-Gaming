using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoors : MonoBehaviour {

    private Animator animator;

    void Start () {

        animator = GetComponent<Animator>();		
	}

    public void OpenDoors(bool state)
    {
        if (state) animator.SetBool("BL_OpenDoor", true);
        else animator.SetBool("BL_OpenDoor", false);
    }
}
