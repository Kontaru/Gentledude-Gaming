using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPart : MonoBehaviour {

    public static QuestPart instance;
    public bool BL_IsInteractable;
    public bool BL_MinigameComplete;
    public bool BL_MinigameFail;
	
	// Update is called once per frame
	virtual public void Update () {
        if (GameManager.instance.PixelMode) return;
        if (BL_IsInteractable)
            instance = this;
    }

    public virtual void CheckEndCondition()
    {
        if (BL_MinigameComplete)
        {
            BL_IsInteractable = false;
        }
    }
}
