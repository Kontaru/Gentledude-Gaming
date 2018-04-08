using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_CI_Checker : HYL_OS_Minigame_Handler
{

    public CI_PlayerBehaviour CIgame;
    // Update is called once per frame
    override public void Update()
    {
        base.Update();

        BL_MinigameComplete = CIgame.BL_GameComplete;
        BL_MinigameFail = CIgame.BL_MinigameFailed;
    }
}
