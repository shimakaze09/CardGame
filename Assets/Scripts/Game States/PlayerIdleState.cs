using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    public override void Enter()
    {
        container.GetAspect<AttackSystem>().Refresh();
        Temp_AutoChangeTurnForAI();
    }

    public override void Exit()
    {
        container.GetAspect<AttackSystem>().Clear();
    }

    private void Temp_AutoChangeTurnForAI()
    {
        if (container.GetMatch().CurrentPlayer.mode != ControlModes.Local)
            container.GetAspect<MatchSystem>().ChangeTurn();
    }
}