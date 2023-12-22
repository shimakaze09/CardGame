using TheLiquidFire.AspectContainer;
using UnityEngine;

public class GameViewSystem : MonoBehaviour, IAspect
{
    private IContainer _container;

    private ActionSystem actionSystem;

    private void Awake()
    {
        container.Awake();
        actionSystem = container.GetAspect<ActionSystem>();
        Temp_SetupSinglePlayer();
    }

    private void Start()
    {
        container.ChangeState<PlayerIdleState>();
    }

    private void Update()
    {
        actionSystem.Update();
    }

    public IContainer container
    {
        get
        {
            if (_container == null)
            {
                _container = GameFactory.Create();
                _container.AddAspect(this);
            }

            return _container;
        }
        set => _container = value;
    }

    private void Temp_SetupSinglePlayer()
    {
        var match = container.GetMatch();
        match.players[0].mode = ControlModes.Local;
        match.players[1].mode = ControlModes.Computer;

        foreach (var p in match.players)
        {
            for (var i = 0; i < Player.maxDeck; ++i)
            {
                var card = new Minion();
                card.name = "Card " + i;
                card.cost = Random.Range(1, 10);
                card.maxHitPoints = card.hitPoints = Random.Range(1, card.cost);
                card.attack = card.cost - card.hitPoints;
                card.allowedAttacks = 1;
                card.ownerIndex = p.index;
                if (i % 3 == 0)
                {
                    card.AddAspect(new Taunt());
                    card.text = "Taunt";
                }

                p[Zones.Deck].Add(card);
            }

            var hero = new Hero();
            hero.hitPoints = hero.maxHitPoints = 30;
            hero.allowedAttacks = 1;
            hero.ownerIndex = p.index;
            hero.zone = Zones.Hero;
            p.hero.Add(hero);
        }
    }
}