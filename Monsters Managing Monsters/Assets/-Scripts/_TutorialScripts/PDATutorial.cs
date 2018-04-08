using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class PDATutorial : QuestPart {

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    override public void Update()
    {
        base.Update();
        if (Interactable() && Input.GetKeyDown(KeyCode.E))
        {
            Fungus.Flowchart.BroadcastFungusMessage("PDATutorial");
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
}
