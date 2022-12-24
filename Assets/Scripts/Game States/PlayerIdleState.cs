using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    public override void Enter()
    {
        Temp_AutoChangeTurnForAI();
    }

    private void Temp_AutoChangeTurnForAI()
    {
        if (container.GetMatch().CurrentPlayer.mode != ControlModes.Local)
            container.GetAspect<MatchSystem>().ChangeTurn();
    }
}