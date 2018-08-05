using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PregameManager
{

    private Dice dice;
    private List<Player> players;
    private Dictionary<Player, int> playerToDiceRoll;
    private int highestRoll = 0;

    private Action<Player> onPregameEndedCallback;

    public PregameManager(List<Player> players)
    {
        this.players = players;

    }

    public void Begin(Action<Player> OnPregameEndedCallback)
    {
        onPregameEndedCallback = OnPregameEndedCallback;

        resetAndPlayPregame();

    }

    private void resetAndPlayPregame()
    {
        dice = new Dice();
        playerToDiceRoll = new Dictionary<Player, int>();
        highestRoll = 0;
        Debug.Log("Pregame started! player count: " + players.Count);
        determineWinner();
    }

    private void determineWinner()
    {
        rollDiceForPlayers();
        List<Player> playersWithHighestDiceRoll = getPlayersWithHighestDiceRoll();
        if (isThereATieBetween(playersWithHighestDiceRoll))
        {
            // shrinks the amount of current players to only the players with the highest dice roll
            // So in the next iteration of pregame, they battle each other rather than all the players.
            players = playersWithHighestDiceRoll;
            resetAndPlayPregame();
            return;
        }

        Player winner = getWinner();
        endPregameWithWinner(winner);
    }

    private int rollDiceForPlayers()
    {
        highestRoll = 0;
        foreach (Player p in players)
        {
            int diceRoll = dice.Roll();
            playerToDiceRoll[p] = diceRoll;
            if (highestRoll < diceRoll)
                highestRoll = diceRoll;
        }

        return highestRoll;
    }

    private bool isThereATieBetween(List<Player> playersWithHighestDiceRoll)
    {      
        if (playersWithHighestDiceRoll.Count > 1)
        {
            return true;
        }
        return false;
    }

    private List<Player> getPlayersWithHighestDiceRoll()
    {
        return playerToDiceRoll.Where(i => i.Value == highestRoll).Select(i => i.Key).ToList();
    }


    private Player getWinner()
    {
        return getPlayersWithHighestDiceRoll()[0];
    }

    private void endPregameWithWinner(Player winner)
    {
        if (onPregameEndedCallback != null)
            onPregameEndedCallback(winner);
    }
}

