﻿using System.Collections.Generic;

public class Player
{
    public const int maxDeck = 30;
    public const int maxHand = 10;
    public const int maxBattlefield = 7;
    public const int maxSecrets = 5;

    public readonly int index;
    public List<Card> battlefield = new(maxBattlefield);
    public List<Card> deck = new(maxDeck);
    public int fatigue;
    public List<Card> graveyard = new(maxDeck);
    public List<Card> hand = new(maxHand);

    public List<Card> hero = new(1);
    public Mana mana = new();
    public ControlModes mode;
    public List<Card> secrets = new(maxSecrets);
    public List<Card> weapon = new(1);

    public Player(int index)
    {
        this.index = index;
    }

    public List<Card> this[Zones z]
    {
        get
        {
            switch (z)
            {
                case Zones.Hero:
                    return hero;
                case Zones.Weapon:
                    return weapon;
                case Zones.Deck:
                    return deck;
                case Zones.Hand:
                    return hand;
                case Zones.Battlefield:
                    return battlefield;
                case Zones.Secrets:
                    return secrets;
                case Zones.Graveyard:
                    return graveyard;
                default:
                    return null;
            }
        }
    }
}