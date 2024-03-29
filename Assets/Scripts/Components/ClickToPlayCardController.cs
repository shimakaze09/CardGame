﻿using System.Collections;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

public class ClickToPlayCardController : MonoBehaviour
{
    private CardView activeCardView;
    private Container container;

    private IContainer game;
    private StateMachine stateMachine;

    private void Awake()
    {
        game = GetComponentInParent<GameViewSystem>().container;
        container = new Container();
        stateMachine = container.AddAspect<StateMachine>();
        container.AddAspect(new WaitingForInputState()).owner = this;
        container.AddAspect(new ShowPreviewState()).owner = this;
        container.AddAspect(new ConfirmOrCancelState()).owner = this;
        container.AddAspect(new CancellingState()).owner = this;
        container.AddAspect(new ConfirmState()).owner = this;
        container.AddAspect(new ShowTargetState()).owner = this;
        container.AddAspect(new TargetState()).owner = this;
        container.AddAspect(new ResetState()).owner = this;
        stateMachine.ChangeState<WaitingForInputState>();
    }

    private void OnEnable()
    {
        this.AddObserver(OnClickNotification, Clickable.ClickedNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnClickNotification, Clickable.ClickedNotification);
    }

    private void OnClickNotification(object sender, object args)
    {
        var handler = stateMachine.currentState as IClickableHandler;
        if (handler != null)
            handler.OnClickNotification(sender, args);
    }

    #region Controller States

    private interface IClickableHandler
    {
        void OnClickNotification(object sender, object args);
    }

    private abstract class BaseControllerState : BaseState
    {
        public ClickToPlayCardController owner;
    }

    private class WaitingForInputState : BaseControllerState, IClickableHandler
    {
        public void OnClickNotification(object sender, object args)
        {
            var gameStateMachine = owner.game.GetAspect<StateMachine>();
            if (!(gameStateMachine.currentState is PlayerIdleState))
                return;

            var clickable = sender as Clickable;
            var cardView = clickable.GetComponent<CardView>();
            if (cardView == null ||
                cardView.card.zone != Zones.Hand ||
                cardView.card.ownerIndex != owner.game.GetMatch().currentPlayerIndex)
                return;

            gameStateMachine.ChangeState<PlayerInputState>();
            owner.activeCardView = cardView;
            owner.stateMachine.ChangeState<ShowPreviewState>();
        }
    }

    private class ShowPreviewState : BaseControllerState
    {
        public override void Enter()
        {
            base.Enter();
            owner.StartCoroutine(ShowProcess());
        }

        private IEnumerator ShowProcess()
        {
            var handView = owner.activeCardView.GetComponentInParent<HandView>();
            yield return owner.StartCoroutine(handView.ShowPreview(owner.activeCardView.transform));
            owner.stateMachine.ChangeState<ConfirmOrCancelState>();
        }
    }

    private class ConfirmOrCancelState : BaseControllerState, IClickableHandler
    {
        public void OnClickNotification(object sender, object args)
        {
            var cardView = (sender as Clickable).GetComponent<CardView>();
            if (owner.activeCardView == cardView)
            {
                var target = owner.activeCardView.card.GetAspect<Target>();
                if (target != null)
                    owner.stateMachine.ChangeState<ShowTargetState>();
                else
                    owner.stateMachine.ChangeState<ConfirmState>();
            }
            else
            {
                owner.stateMachine.ChangeState<CancellingState>();
            }
        }
    }

    private class CancellingState : BaseControllerState
    {
        public override void Enter()
        {
            base.Enter();
            owner.StartCoroutine(HideProcess());
        }

        private IEnumerator HideProcess()
        {
            var handView = owner.activeCardView.GetComponentInParent<HandView>();
            yield return owner.StartCoroutine(handView.LayoutCards());
            owner.stateMachine.ChangeState<ResetState>();
        }
    }

    private class ConfirmState : BaseControllerState
    {
        public override void Enter()
        {
            base.Enter();
            var action = new PlayCardAction(owner.activeCardView.card);
            owner.game.Perform(action);
            owner.stateMachine.ChangeState<ResetState>();
        }
    }

    private class ShowTargetState : BaseControllerState
    {
        public override void Enter()
        {
            base.Enter();
            owner.StartCoroutine(HideProcess());
        }

        private IEnumerator HideProcess()
        {
            var handView = owner.activeCardView.GetComponentInParent<HandView>();
            yield return owner.StartCoroutine(handView.LayoutCards());
            owner.stateMachine.ChangeState<TargetState>();
        }
    }

    private class TargetState : BaseControllerState, IClickableHandler
    {
        public void OnClickNotification(object sender, object args)
        {
            var target = owner.activeCardView.card.GetAspect<Target>();
            var cardView = (sender as Clickable).GetComponent<BattlefieldCardView>();
            if (cardView != null)
                target.selected = cardView.card;
            else
                target.selected = null;
            owner.stateMachine.ChangeState<ConfirmState>();
        }
    }

    private class ResetState : BaseControllerState
    {
        public override void Enter()
        {
            base.Enter();
            owner.stateMachine.ChangeState<WaitingForInputState>();
            if (!owner.game.GetAspect<ActionSystem>().IsActive)
                owner.game.ChangeState<PlayerIdleState>();
        }
    }

    #endregion
}