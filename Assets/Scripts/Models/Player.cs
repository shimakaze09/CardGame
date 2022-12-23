using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public const int MaxDeck = 30;
    public const int MaxHand = 10;
    public const int MaxBattlefields = 7;
    public const int MaxSecrets = 5;

    public readonly int index;
    public ControlModes mode;
    public Mana mana = new();
    public int fatigue;

    public List<Card> hero = new(1);
    public List<Card> weapon = new(1);
    public List<Card> deck = new(MaxDeck);
    public List<Card> hand = new(MaxHand);
    public List<Card> battlefield = new(MaxBattlefields);
    public List<Card> secrets = new(MaxSecrets);
    public List<Card> graveyard = new(MaxDeck);

    public List<Card> this[Zones z]
    {
        get
        {
            return z switch
            {
                Zones.Hero => hero,
                Zones.Weapon => weapon,
                Zones.Deck => deck,
                Zones.Hand => hand,
                Zones.Battlefield => battlefield,
                Zones.Secrets => secrets,
                Zones.Graveyard => graveyard,
                _ => null
            };
        }
    }

    public Player(int index)
    {
        this.index = index;
    }
}