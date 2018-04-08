using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_PC_Checker : HYL_OS_Minigame_Handler {

    public PC_PlayerBehaviour PCgame;
    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        BL_MinigameComplete = PCgame.BL_GameComplete;
        BL_MinigameFail = PCgame.BL_MinigameFailed;
    }
}
