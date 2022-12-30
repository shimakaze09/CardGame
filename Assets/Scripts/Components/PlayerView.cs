using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public DeckView deck;
    public HandView hand;
    public TableView table;
    public HeroView hero;
    public GameObject cardPrefab;
    public Player player { get; private set; }

    public void SetPlayer(Player player)
    {
        this.player = player;
        var heroCard = player.hero[0] as Hero;
        hero.SetHero(heroCard);
    }

    public GameObject GetMatch(Card card)
    {
        return card.zone switch
        {
            Zones.Battlefield => table.GetMatch(card),
            Zones.Hero => hero.gameObject,
            _ => null
        };
    }
}