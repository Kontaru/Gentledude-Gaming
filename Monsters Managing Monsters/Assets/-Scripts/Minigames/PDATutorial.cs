using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class PDATutorial : QuestPart {

    private Camera cam;
    public Flowchart flow;
    public string[] flavourText;

    private void Start()
    {
        cam = Camera.main;
    }

    override public void Update()
    {
        base.Update();
        if (Interactable())
        {
            if (BL_IsInteractable)
            {
                if (Input.GetKeyDown(KeyCode.E)) Fungus.Flowchart.BroadcastFungusMessage("PDATutorial");
                if (flow.GetBooleanVariable("bl_textCycleOver") == true) BL_MinigameComplete = true;
            }else
            {
                if (Input.GetKeyDown(KeyCode.E)) FungusDirector.instance.IdleNPC(FlavourText());
            }
        }


    }

    bool Interactable()
    {

        Vector3 OnScreenPos = cam.WorldToViewportPoint(gameObject.transform.position);

        float x = OnScreenPos.x;
        float y = OnScreenPos.y;
        float z = OnScreenPos.z;

        if (z > 0 && x > 0.4 && x < 0.6 && y > 0.4 && y < 0.4)
        {
            return true;
        }
        else
            return false;
    }

    string FlavourText()
    {
        int rand = Random.Range(0, flavourText.Length - 1);
        return flavourText[rand];
    }
}
