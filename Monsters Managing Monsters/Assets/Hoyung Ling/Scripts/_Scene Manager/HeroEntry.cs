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
        if (BL_Confirmed) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag != "Monster") return;

            if (Input.GetMouseButtonDown(0))
            {
                monster = hit.transform.gameObject;                
                delta = monster.transform.position - hit.point;
                DropHighlight(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                DropHighlight(false);
                monster = null;
            }

            if (Input.GetMouseButton(0))
            {
                if (monster != null)
                {
                    monster.transform.position = new Vector3(hit.point.x + delta.x, monster.transform.position.y + 5, hit.point.z + delta.z);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmationScreen();
        }
    }

    private void DropHighlight(bool flag)
    {
        GameObject parent = monster.transform.Find("Graphic").gameObject;
        GameObject highlight = parent.transform.GetChild(1).gameObject;

        if (flag) highlight.SetActive(true);
        else highlight.SetActive(false);
    }

    private void ConfirmationScreen()
    {
        Debug.Log("Confirm monster positions?");
        //BL_Confirmed = true;
    }
}
