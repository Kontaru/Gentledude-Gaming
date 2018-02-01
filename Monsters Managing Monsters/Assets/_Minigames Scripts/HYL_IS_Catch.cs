using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_IS_Catch : QuestPart {

    public GameObject GO_NPC;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

        if (BL_IsInteractable)
        {
            GO_NPC.SetActive(true);
        }
        else
        {
            GO_NPC.SetActive(false);
        }

        if (GO_NPC.GetComponent<HYL_IS_CatchObject>().BL_Captured)
            BL_MinigameComplete = true;
    }
}
