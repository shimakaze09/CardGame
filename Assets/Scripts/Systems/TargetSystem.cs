using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Extensions;
using TheLiquidFire.Notifications;

public class TargetSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnValidatePlayCard, Global.ValidateNotification<PlayCardAction>());
    }

    public void Destroy()
    {
        this.RemoveObserver(OnValidatePlayCard, Global.ValidateNotification<PlayCardAction>());
    }

    public void AutoTarget(Card card, ControlModes mode)
    {
        var target = card.GetAspect<Target>();
        if (target == null)
            return;
        var mark = mode == ControlModes.Computer ? target.preferred : target.allowed;
        var candidates = GetMarks(target, mark);
        target.selected = candidates.Count > 0 ? candidates.Random() : null;
    }

    private List<Card> GetMarks(Target source, Mark mark)
    {
        var marks = new List<Card>();
        var players = GetPlayers(source, mark);
        foreach (var player in players)
        {
            var cards = GetCards(source, mark, player);
            marks.AddRange(cards);
        }

        return marks;
    }

    private List<Player> GetPlayers(Target source, Mark mark)
    {
        var card = source.container as Card;
        var dataSystem = container.GetAspect<DataSystem>();
        var players = new List<Player>();
        var pair = new Dictionary<Alliance, Player>
        {
            { Alliance.Ally, dataSystem.match.players[card.ownerIndex] },
            { Alliance.Enemy, dataSystem.match.players[1 - card.ownerIndex] }
        };
        foreach (var key in pair.Keys)
            if (mark.alliance.Contains(key))
                players.Add(pair[key]);
        return players;
    }

    private List<Card> GetCards(Target source, Mark mark, Player player)
    {
        var cards = new List<Card>();
        var zones = new[]
        {
            Zones.Hero,
            Zones.Weapon,
            Zones.Deck,
            Zones.Hand,
            Zones.Battlefield,
            Zones.Secrets,
            Zones.Graveyard
        };
        foreach (var zone in zones)
            if (mark.zones.Contains(zone))
                cards.AddRange(player[zone]);
        return cards;
    }

    private void OnValidatePlayCard(object sender, object args)
    {
        var playCardAction = sender as PlayCardAction;
        var target = playCardAction.card.GetAspect<Target>();
        if (target == null || (target.required == false && target.selected == null))
            return;
        var validator = args as Validator;
        var candidates = GetMarks(target, target.allowed);
        if (!candidates.Contains(target.selected)) validator.Invalidate();
    }
}