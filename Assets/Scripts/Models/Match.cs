using System.Collections.Generic;

public class Match
{
    public const int PlayerCount = 2;
    public int currentPlayerIndex;

    public List<Player> players = new(PlayerCount);

    public Match()
    {
        for (var i = 0; i < PlayerCount; ++i) players.Add(new Player(i));
    }

    public Player CurrentPlayer => players[currentPlayerIndex];

    public Player OpponentPlayer => players[1 - currentPlayerIndex];
}