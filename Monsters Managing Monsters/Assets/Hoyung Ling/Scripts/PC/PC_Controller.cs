using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Controller : MonoBehaviour {

    //Bool
    public bool BL_Staggered = false;
    public bool BL_StaggerToggle = false;
    public float FL_StaggerTimer = 1.0f;
    public bool BL_IsMoving;

    //Controllers
    PC_Melee CC_Melee;
    PC_Move CC_Move;

	// Use this for initialization
	void Start () {
        AudioManager.instance.Stop("Theme");
        AudioManager.instance.Play("Dungeon Music");

        CC_Melee = GetComponent<PC_Melee>();
        CC_Move = GetComponent<PC_Move>();
    }
	
	// Update is called once per frame
	void Update () {
        BL_IsMoving = CC_Move.isMoving;
    }

    IEnumerator StaggerTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        BL_Staggered = false;
        BL_StaggerToggle = false;
    }
}
