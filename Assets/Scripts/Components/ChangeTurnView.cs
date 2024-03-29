﻿using System.Collections;
using TheLiquidFire.Animation;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class ChangeTurnView : MonoBehaviour
{
    [SerializeField] private Transform yourTurnBanner;
    [SerializeField] private ChangeTurnButtonView buttonView;
    private IContainer game;

    private void Awake()
    {
        game = GetComponentInParent<GameViewSystem>().container;
    }

    private void OnEnable()
    {
        this.AddObserver(OnPrepareChangeTurn, Global.PrepareNotification<ChangeTurnAction>(), game);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnPrepareChangeTurn, Global.PrepareNotification<ChangeTurnAction>(), game);
    }

    public void ChangeTurnButtonPressed()
    {
        if (CanChangeTurn())
        {
            var system = game.GetAspect<MatchSystem>();
            system.ChangeTurn();
        }
        // TODO: Play an error input sound effect?
    }

    private bool CanChangeTurn()
    {
        var stateMachine = game.GetAspect<StateMachine>();
        if (!(stateMachine.currentState is PlayerIdleState))
            return false;

        var player = game.GetMatch().CurrentPlayer;
        if (player.mode != ControlModes.Local)
            return false;

        return true;
    }

    private void OnPrepareChangeTurn(object sender, object args)
    {
        var action = args as ChangeTurnAction;
        action.perform.viewer = ChangeTurnViewer;
    }

    private IEnumerator ChangeTurnViewer(IContainer game, GameAction action)
    {
        var dataSystem = game.GetAspect<DataSystem>();
        var changeTurnAction = action as ChangeTurnAction;
        var targetPlayer = dataSystem.match.players[changeTurnAction.targetPlayerIndex];

        var banner = ShowBanner(targetPlayer);
        var button = FlipButton(targetPlayer);
        var isAnimating = true;

        do
        {
            var bannerOn = banner.MoveNext();
            var buttonOn = button.MoveNext();
            isAnimating = bannerOn || buttonOn;
            yield return null;
        } while (isAnimating);
    }

    private IEnumerator ShowBanner(Player targetPlayer)
    {
        if (targetPlayer.mode != ControlModes.Local)
            yield break;

        var tweener = yourTurnBanner.ScaleTo(Vector3.one, 0.25f, EasingEquations.EaseOutBack);
        while (tweener.IsPlaying) yield return null;

        tweener = yourTurnBanner.Wait(1f);
        while (tweener.IsPlaying) yield return null;

        tweener = yourTurnBanner.ScaleTo(Vector3.zero, 0.25f, EasingEquations.EaseInBack);
        while (tweener.IsPlaying) yield return null;
    }

    private IEnumerator FlipButton(Player targetPlayer)
    {
        var up = Quaternion.identity;
        var down = Quaternion.Euler(new Vector3(180, 0, 0));
        var targetRotation = targetPlayer.mode == ControlModes.Local ? up : down;
        var tweener = buttonView.rotationHandle.RotateTo(targetRotation, 0.5f, EasingEquations.EaseOutBack);
        while (tweener.IsPlaying) yield return null;
    }
}