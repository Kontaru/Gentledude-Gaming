using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroEntry : MonoBehaviour {

    public static HeroEntry instance;

    public bool SpawnHeroes;
    public bool BL_HeroArrival;
    private bool BL_Confirmed;

    private GameObject monster;

    #region Typical Singleton Format

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.PixelMode) return;
        if (BL_HeroArrival) MonsterPlacement();       
    }

    private void MonsterPlacement()
    {        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                monster = hit.transform.gameObject;                
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            monster = null;
        }

        if (monster != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            monster.transform.position = new Vector3(mousePos.x, monster.transform.position.y + 5, mousePos.z);
        } 
    }

    private void ConfirmationScreen()
    {

    }
}
