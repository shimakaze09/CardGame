using TheLiquidFire.AspectContainer;
using TheLiquidFire.Extensions;
using TheLiquidFire.Notifications;
using UnityEngine;

public class PlayerSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), container);
        this.AddObserver(OnPerformDrawCards, Global.PerformNotification<DrawCardsAction>(), container);
        this.AddObserver(OnPerformFatigue, Global.PerformNotification<FatigueAction>(), container);
        this.AddObserver(OnPerformOverDraw, Global.PerformNotification<OverdrawAction>(), container);
        this.AddObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>(), container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), container);
        this.RemoveObserver(OnPerformDrawCards, Global.PerformNotification<DrawCardsAction>(), container);
        this.RemoveObserver(OnPerformFatigue, Global.PerformNotification<FatigueAction>(), container);
        this.RemoveObserver(OnPerformOverDraw, Global.PerformNotification<OverdrawAction>(), container);
        this.RemoveObserver(OnPerformPlayCard, Global.PerformNotification<PlayCardAction>(), container);
    }

    private void OnPerformChangeTurn(object sender, object args)
    {
        var action = args as ChangeTurnAction;
        var match = container.GetAspect<DataSystem>().match;
        var player = match.players[action.targetPlayerIndex];
        DrawCards(player, 1);
    }

    private void OnPerformDrawCards(object sender, object args)
    {
        var action = args as DrawCardsAction;

        var deckCount = action.player[Zones.Deck].Count;
        var fatigueCount = Mathf.Max(action.amount - deckCount, 0);
        for (var i = 0; i < fatigueCount; ++i)
        {
            var fatigueAction = new FatigueAction(action.player);
            container.AddReaction(fatigueAction);
        }

        var roomInHand = Player.maxHand - action.player[Zones.Hand].Count;
        var overDraw = Mathf.Max(action.amount - fatigueCount - roomInHand, 0);
        if (overDraw > 0)
        {
            var overDrawAction = new OverdrawAction(action.player, overDraw);
            container.AddReaction(overDrawAction);
        }

        var drawCount = action.amount - fatigueCount - overDraw;
        action.cards = action.player[Zones.Deck].Draw(drawCount);
        foreach (var card in action.cards)
            ChangeZone(card, Zones.Hand);
    }

    private void OnPerformFatigue(object sender, object args)
    {
        var action = args as FatigueAction;
        action.player.fatigue++;

        var damageTarget = action.player.hero[0] as IDestructable;
        var damageAction = new DamageAction(damageTarget, action.player.fatigue);
        container.AddReaction(damageAction);
    }

    private void OnPerformOverDraw(object sender, object args)
    {
        var action = args as OverdrawAction;
        action.cards = action.player[Zones.Deck].Draw(action.amount);
        foreach (var card in action.cards)
            ChangeZone(card, Zones.Graveyard);
    }

    private void OnPerformPlayCard(object sender, object args)
    {
        var action = args as PlayCardAction;
        if (action.card.zone == Zones.Hand)
            ChangeZone(action.card, Zones.Graveyard);
    }

    private void DrawCards(Player player, int amount)
    {
        var action = new DrawCardsAction(player, amount);
        container.AddReaction(action);
    }

    private void ChangeZone(Card card, Zones zone, Player toPlayer = null)
    {
        var cardSystem = container.GetAspect<CardSystem>();
        cardSystem.ChangeZone(card, zone, toPlayer);
    }
}