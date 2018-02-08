using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MainTextBox : MonoBehaviour {

	void Awake() {
        NPCInteraction.CC_Dialogue = GetComponent<Flowchart>();
    }
}
