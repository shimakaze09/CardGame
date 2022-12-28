using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using UnityEngine;

/*
public class DragAndDropController : MonoBehaviour
{
    private IContainer game;
    private Container container;
    private StateMachine stateMachine;
    private GameObject activeObject;
    private Vector3 initialPosition;

    private void Awake()
    {
        game = GetComponentInParent<GameViewSystem>().container;
        container = new Container();
        stateMachine = container.AddAspect<StateMachine>();
        container.AddAspect(new WaitingForInputState()).owner = this;
        container.AddAspect(new DraggingState()).owner = this;
        container.AddAspect(new DroppingState()).owner = this;
        container.AddAspect(new ResetState()).owner = this;
        stateMachine.ChangeState<WaitingForInputState>();
    }

    private void OnEnable()
    {
        this.AddObserver(OnBeginDragNotification, Dragable.BeginDragNotification);
        this.AddObserver(OnEndDragNotification, Dragable.EndDragNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnBeginDragNotification, Dragable.BeginDragNotification);
        this.RemoveObserver(OnEndDragNotification, Dragable.EndDragNotification);
    }

    private void OnBeginDragNotification(object sender, object args)
    {
        var handler = stateMachine.currentState as IDragDropHandler;
        handler?.OnBeginDragNotification(sender, args);
    }

    private void OnEndDragNotification(object sender, object args)
    {
        var handler = stateMachine.currentState as IDragDropHandler;
        handler?.OnEndDragNotification(sender, args);
    }

    #region Controller States

    private interface IDragDropHandler
    {
        void OnBeginDragNotification(object sender, object args);
        void OnEndDragNotification(object sender, object args);
    }

    private abstract class BaseControllerState : BaseState
    {
        public DragAndDropController owner;
    }

    private class WaitingForInputState : BaseControllerState, IDragDropHandler
    {
        public void OnBeginDragNotification(object sender, object args)
        {
            var gameStateMachine = owner.game.GetAspect<StateMachine>();
            if (gameStateMachine.currentState is not PlayerIdleState) return;

            var dragable = sender as Dragable;
            var cardView = dragable.GetComponent<CardView>();
            if (cardView == null || cardView.card.zone != Zones.Hand ||
                cardView.card.ownerIndex != owner.game.GetMatch().currentPlayerIndex) return;

            gameStateMachine.ChangeState<PlayerInputState>();
            owner.activeObject = dragable.gameObject;
            owner.initialPosition = owner.activeObject.transform.position;
            owner.stateMachine.ChangeState<DraggingState>();
        }

        public void OnEndDragNotification(object sender, object args)
        {
        }
    }

    private class DraggingState : BaseControllerState, IDragDropHandler
    {
        public void OnBeginDragNotification(object sender, object args)
        {
        }

        public void OnEndDragNotification(object sender, object args)
        {
            owner.stateMachine.ChangeState<DroppingState>();
        }
    }

    private class DroppingState : BaseControllerState
    {
        public override void Enter()
        {
            base.Enter();
            owner.StartCoroutine(DropProcess());
        }

        private IEnumerator DropProcess()
        {
            // Check if the active object has been dropped onto a valid drop zone
            var dropZone = GetDropZone(owner.activeObject);
            if (dropZone != null)
            {
                // Perform actions based on the type of drop zone and the object being dropped
                var cardView = owner.activeObject.GetComponent<CardView>();
                if (dropZone.CompareTag("Board"))
                    // Play the card onto the board
                    yield return PlayCard(cardView, dropZone.GetComponent<BoardView>());
                else if (dropZone.CompareTag("Hand"))
                    // Return the card to the hand
                    yield return ReturnCardToHand(cardView, dropZone.GetComponent<HandView>());
            }
            else
            {
                // Return the active object to its initial position if it wasn't dropped onto a valid drop zone
                owner.activeObject.transform.position = owner.initialPosition;
            }

            // Reset the controller state and allow player input again
            owner.stateMachine.ChangeState<ResetState>();
        }

        private DropZone GetDropZone(GameObject droppedObject)
        {
            // Check if the dropped object is within the boundaries of a drop zone
            var dropZones = FindObjectsOfType<DropZone>();
            return dropZones.FirstOrDefault(dropZone => dropZone.IsWithinBounds(droppedObject.transform.position));
        }

        private IEnumerator PlayCard(CardView cardView, BoardView boardView)
        {
            // Play the card onto the board and update the board view
            yield return boardView.PlayCard(cardView);
        }

        private IEnumerator ReturnCardToHand(CardView cardView, HandView handView)
        {
            // Return the card to the hand and update the hand view
            yield return handView.ReturnCard(cardView);
        }
    }

    private class ResetState : BaseControllerState
    {
        public override void Enter()
        {
            base.Enter();
            owner.activeObject = null;
            owner.stateMachine.ChangeState<WaitingForInputState>();
        }
    }

    #endregion
} */