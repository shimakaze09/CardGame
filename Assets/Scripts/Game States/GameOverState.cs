using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using UnityEngine;

public class GameOverState : BaseState
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Game Over");
    }
}