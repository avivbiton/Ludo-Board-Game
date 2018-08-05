using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameRound : MonoBehaviour, IGameInputReceiver
{

    private static IGameInputReceiver instance;

    private Player[] currentPlayers;
    private Player currentPlayerTurn;
    private Dice dice;
    private RoundState roundState;

    private List<LegalMove> legalMoves;

    public static IGameInputReceiver GetInputReceiver()
    {
        return instance;
    }

    public void SetupPlayers(Player[] players)
    {
        currentPlayers = players;
    }

    public void StartRound()
    {

        instance = this;

        dice = new Dice();
        roundState = RoundState.Pregame;
        preparePregame();

    }

    public void InputMove(Player player, LudoPiece piece, BoardTile tile)
    {
        validateAndThrowInvalidInput(player);

        acceptPlayerInput(piece, tile);
    }


    public void InputPutLudoOutOfPlay(Player player, LudoPiece piece)
    {
        validateAndThrowInvalidInput(player);
        acceptInputMoveOutOfPlay(piece);
    }

    private void preparePregame()
    {
        PregameManager pregame = new PregameManager(currentPlayers.ToList());
        pregame.Begin(onPregameEnded);
    }

    private void onPregameEnded(Player winner)
    {
        currentPlayerTurn = winner;
        startGame();
    }

    private void startGame()
    {
        roundState = RoundState.Active;
        beginPlayerTurn();
    }

    private void beginPlayerTurn()
    {
        invokeStartOfTurnEvent();

        dice.Roll();
        getLegalMoves();
        if (hasValidMoves())
            requestAndWaitForInput();
        else
            advanceToNextTurn();
    }

    private void acceptPlayerInput(LudoPiece ludoToMove, BoardTile tile)
    {
        ludoToMove.MoveToTile(tile);
        advanceToNextTurn();
    }

    private void acceptInputMoveOutOfPlay(LudoPiece piece)
    {
        piece.MoveOutOfPlay();
        if (hasPlayerWon())
        {
            invokePlayerWonEvent();
            endGame();
        }
        else
            advanceToNextTurn();
    }


    private void advanceToNextTurn()
    {
        invokeEndOfTurnEvent();
        currentPlayerTurn = currentPlayers[getNextPlayerID()];
        beginPlayerTurn();
    }


    private void requestAndWaitForInput()
    {
        PlayerDecisionEvent decisionEvent = new PlayerDecisionEvent()
        {
            player = currentPlayerTurn,
            legalMoves = this.legalMoves
        };

        decisionEvent.FireEvent();
    }

    private bool hasPlayerWon()
    {
        foreach (LudoPiece ludo in currentPlayerTurn.GetPlayerLudos())
            if (ludo.IsInPlay())
                return false;
        return true;

    }

    private void endGame()
    {
        Destroy(this);
    }

    private void getLegalMoves()
    {
        legalMoves = new List<LegalMove>();
        int diceResult = dice.GetDiceResult();
        foreach (LudoPiece ludo in currentPlayerTurn.GetPlayerLudos())
            if (ludo.IsInPlay())
                legalMoves.Add(
                    new LegalMove(ludo,
                    ludo.GetAvailableMoveForRoll(diceResult),
                    ludo.CanMoveOutOfPlay(diceResult))
                    );
    }

    private bool hasValidMoves()
    {
        foreach (LegalMove move in legalMoves)
        {
            if (move.AvailableMove != null || move.CanMoveOutOfPlay)
                return true;
        }
        return false;
    }

    private int getNextPlayerID()
    {
        return ( currentPlayerTurn.GetPlayerID() + 1 ) > ( currentPlayers.Count() - 1 ) ? 0 : currentPlayerTurn.GetPlayerID() + 1;
    }


    private void invokeStartOfTurnEvent()
    {
        StartOfTurnEvent turnEvent = new StartOfTurnEvent()
        {
            player = currentPlayerTurn
        };

        turnEvent.FireEvent();
    }

    private void invokeEndOfTurnEvent()
    {
        EndOfTurnEvent endOfTurn = new EndOfTurnEvent() { player = currentPlayerTurn };
        endOfTurn.FireEvent();
    }

    private void invokePlayerWonEvent()
    {
        var wonEvent = new PlayerWonEvent() { player = currentPlayerTurn };
        wonEvent.FireEvent();
    }

    private void validateAndThrowInvalidInput(Player player)
    {
        if (player != currentPlayerTurn)
        {
            Debug.LogError("Player ID " + player.GetPlayerID() + " is trying to send move Input while it's not their turn. Current turn is for player ID " + currentPlayerTurn.GetPlayerID());
        }
    }



}
