using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_SS_Checker : HYL_OS_Minigame_Handler {

    public SS_PlayerBehaviour SSgame;
    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        BL_MinigameComplete = SSgame.BL_GameComplete;
        BL_MinigameFail = SSgame.BL_MinigameFailed;
    }
}
