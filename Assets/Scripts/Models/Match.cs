using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match
{
    public const int PlayerCount = 2;

    public List<Player> players = new(PlayerCount);
    public int currentPlayerIndex;

    public Player CurrentPlayer => players[currentPlayerIndex];

    public Player OpponentPlayer => players[1 - currentPlayerIndex];

    public Match()
    {
        for (var i = 0; i < PlayerCount; i++) players.Add(new Player(i));
    }
}