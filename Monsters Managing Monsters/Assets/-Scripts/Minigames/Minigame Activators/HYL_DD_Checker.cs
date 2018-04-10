using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HYL_DD_Checker : HYL_OS_Minigame_Handler
{
    public DD_PlayerBehaviour DDgame;
	// Update is called once per frame
	override public void Update () {

        base.Update();

        BL_MinigameComplete = DDgame.BL_GameComplete;
        BL_MinigameFail = DDgame.BL_MinigameFailed;
	}
}
