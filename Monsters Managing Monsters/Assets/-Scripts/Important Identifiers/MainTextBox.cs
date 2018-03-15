using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MainTextBox : MonoBehaviour {

	void Start() {
        NPCInteraction.CC_Dialogue = GetComponent<Flowchart>();
        HeroEntry.instance.flowchart = GetComponent<Flowchart>();
    }
}
