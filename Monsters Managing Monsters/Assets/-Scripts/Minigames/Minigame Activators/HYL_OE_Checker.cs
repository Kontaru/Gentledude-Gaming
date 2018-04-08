using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_OE_Checker : HYL_OS_Minigame_Handler {

    public OE_PlayerBehaviour OEgame;
    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        BL_MinigameComplete = OEgame.BL_GameComplete;
        BL_MinigameFail = OEgame.BL_MinigameFailed;
    }
}
