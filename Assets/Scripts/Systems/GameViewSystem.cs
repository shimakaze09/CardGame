using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public class GameViewSystem : MonoBehaviour, IAspect
{
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

    private IContainer _container;

    private ActionSystem actionSystem;

    private void Awake()
    {
        container.Awake();
        actionSystem = container.GetAspect<ActionSystem>();
    }

    private void Start()
    {
        Temp_SetupSinglePlayer();
        container.ChangeState<PlayerIdleState>();
    }

    private void Update()
    {
        actionSystem.Update();
    }

    private void Temp_SetupSinglePlayer()
    {
        var match = container.GetMatch();
        match.players[0].mode = ControlModes.Local;
        match.players[1].mode = ControlModes.Computer;

        foreach (var p in match.players)
            for (var i = 0; i < Player.MaxDeck; i++)
            {
                var card = new Minion
                {
                    name = "Card" + i,
                    cost = Random.Range(1, 10)
                };
                card.maxHitPoints = card.hitPoints = Random.Range(1, card.cost);
                card.attack = card.cost - card.hitPoints;
                p[Zones.Deck].Add(card);
            }
    }
}