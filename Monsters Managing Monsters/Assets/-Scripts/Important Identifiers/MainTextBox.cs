using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MainTextBox : MonoBehaviour {

	void Start() {
        if(NPCInteraction.CC_Dialogue != null)
            NPCInteraction.CC_Dialogue = GetComponent<Flowchart>();
        if(HeroEntry.instance != null)
            HeroEntry.instance.flowchart = GetComponent<Flowchart>();
    }
}
