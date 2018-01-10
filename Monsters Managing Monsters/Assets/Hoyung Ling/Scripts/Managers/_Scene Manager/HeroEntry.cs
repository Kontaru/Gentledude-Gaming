using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroEntry : MonoBehaviour {

    public static HeroEntry instance;

    public bool SpawnHeroes;
    public bool BL_HeroArrival;
    private bool BL_Confirmed;

    private GameObject monster;
    private Vector3 delta;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag != "Monster") return;

            if (Input.GetMouseButtonDown(0))
            {
                monster = hit.transform.gameObject;
                monster.transform.position = new Vector3(monster.transform.position.x, monster.transform.position.y + 5, monster.transform.position.z);
                delta = monster.transform.position - hit.point;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                monster = null;
            }

            if (Input.GetMouseButton(0))
            {
                if (monster != null)
                {
                    monster.transform.position = hit.point + delta;                    
                }
            }
        }        
    }

    private void ConfirmationScreen()
    {

    }
}
