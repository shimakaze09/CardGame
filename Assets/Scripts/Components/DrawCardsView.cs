using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Extensions;
using TheLiquidFire.Notifications;
using UnityEngine;

public class DrawCardsView : MonoBehaviour
{
    private void OnEnable()
    {
        this.AddObserver(OnPrepareDrawCards, Global.PrepareNotification<DrawCardsAction>());
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnPrepareDrawCards, Global.PrepareNotification<DrawCardsAction>());
    }

    private void OnPrepareDrawCards(object sender, object args)
    {
        var action = args as DrawCardsAction;
        action.perform.viewer = DrawCardsViewer;
    }

    private IEnumerator DrawCardsViewer(IContainer game, GameAction action)
    {
        yield return true; // perform the action logic so that we know what cards have been drawn
        var drawAction = action as DrawCardsAction;
        var boardView = GetComponent<BoardView>();
        var playerView = boardView.playerViews[drawAction.player.index];

        for (var i = 0; i < drawAction.cards.Count; i++)
        {
            var deckSize = action.player[Zones.Deck].Count + drawAction.cards.Count - (i + 1);
            playerView.deck.ShowDeckSize((float)deckSize / (float)Player.MaxDeck);

            var cardView = boardView.cardPooler.Dequeue().GetComponent<CardView>();
            cardView.card = drawAction.cards[i];
            cardView.transform.ResetParent(playerView.hand.transform);
            cardView.transform.position = playerView.deck.topCard.position;
            cardView.transform.rotation = playerView.deck.topCard.rotation;
            cardView.gameObject.SetActive(true);

            var showPreview = action.player.mode == ControlModes.Local;
            var addCard = playerView.hand.AddCard(cardView.transform, showPreview);
            while (addCard.MoveNext()) yield return null;
        }
    }
}