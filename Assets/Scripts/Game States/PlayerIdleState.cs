using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    public const string EnterNotification = "PlayerIdleState.EnterNotification";
    public const string ExitNotification = "PlayerIdleState.ExitNotification";

    public override void Enter()
    {
        container.GetAspect<AttackSystem>().Refresh();
        Temp_AutoChangeTurnForAI();
        this.PostNotification(EnterNotification);
    }

    public override void Exit()
    {
        this.PostNotification(ExitNotification);
    }

    private void Temp_AutoChangeTurnForAI()
    {
        if (container.GetMatch().CurrentPlayer.mode != ControlModes.Local)
            container.GetAspect<MatchSystem>().ChangeTurn();
    }
}