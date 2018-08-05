using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LudoPiece : MonoBehaviour
{

    private Player owner;
    private BoardTile currentTile;
    private bool isInPlay;
    private GameObject visualObject;

    public static LudoPiece CreateLudoWithGameobject(BoardTile startingTile)
    {
        GameObject gameObject = new GameObject("Ludo piece", typeof(LudoPiece));
        LudoPiece ludoPiece = gameObject.GetComponent<LudoPiece>();

        ludoPiece.isInPlay = true;
        ludoPiece.visualObject = gameObject;

        ludoPiece.MoveToTile(startingTile);

        return ludoPiece;
    }

    public void GrantOwnershipTo(Player player)
    {
        owner = player;
    }

    public void SetVisualApperance(Sprite sprite, Color color)
    {
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        spriteRenderer.sortingLayerName = "PlayerLayer";
    }

    public BoardTile GetAvailableMoveForRoll(int roll)
    {
        if (currentTile.Type == TileType.Stage)
            return calculateMoveForStageRoll(roll);
        else if (currentTile.Type == TileType.Home)
            return calculateMoveForHomeRoll(roll);
        else if (currentTile.Type == TileType.Normal)
            return calculateMoveForRoll(roll);


        throw new System.Exception("GetLegalMovesForRoll failed. LudoPiece.cs");

    }

    public bool CanMoveOutOfPlay(int roll)
    {
        if (currentTile.Type == TileType.Home &&
            currentTile.GetID() == 6 &&
            roll == 6)
            return true;
        return false;
    }

    private BoardTile calculateMoveForStageRoll(int roll)
    {
        if (roll != 6) return null;

        PrivateBoardTiles privateBoardTiles = owner.GetPlayerBoardTiles();
        BoardTile destinationTile = privateBoardTiles.GetStartingTile();
        if (destinationTile.IsOccupied && destinationTile.CurrentLudo.owner == owner) return null;

        return destinationTile;
    }

    private BoardTile calculateMoveForHomeRoll(int roll)
    {
        // If the unit can already move out of play, skip this, we inform the UI about this in other way.
        if (CanMoveOutOfPlay(roll)) return null;

        int currentStage = currentTile.GetID();
        int requiredRoll = currentStage + 1;
        if (roll != requiredRoll)
            return null;
        BoardTile destinationTile = owner.GetPlayerBoardTiles().FindHomeDestinationTile(requiredRoll);
        if (destinationTile.IsOccupied)
            return null;
        return destinationTile;
    }

    private BoardTile calculateMoveForRoll(int roll)
    {
        PrivateBoardTiles playerTiles = owner.GetPlayerBoardTiles();
        if (currentTile == playerTiles.GetEndPointTile())
            return returnCanClimbToHomeTile(roll);

        int destinationTileId = currentTile.GetID() + roll;
        destinationTileId = BoardUtility.CorrectTileIdIfInvalid(destinationTileId);
        BoardTile destinationTile = BoardUtility.FindDestinationTile(destinationTileId);

        List<BoardTile> tilesBetween = BoardUtility.GetTilesBetween(currentTile, destinationTile);
        if (BoardUtility.IsPathBlocked(tilesBetween))
            return null;

        if (tilesBetween.Contains(playerTiles.GetEndPointTile()))
            return null;


        if (destinationTile.IsOccupied && destinationTile.CurrentLudo.owner == owner) return null;
        return destinationTile;
    }

    /// <summary>
    /// returns the board tile when just climbing into the home tile
    /// </summary>
    /// <param name="roll"></param>
    /// <returns></returns>
    private BoardTile returnCanClimbToHomeTile(int roll)
    {
        if (roll == 1)
            return owner.GetPlayerBoardTiles().FindHomeDestinationTile(1);
        return null;

    }


    /// <summary>
    /// Kills the unit and reset it back to staged tile. called when it is "eaten" by other unit.
    /// </summary>
    public void KillAndResetUnit()
    {
        BoardTile freeStageTile = owner.GetPlayerBoardTiles().FindEmptyStageBoardTile();
        freeStageTile.AppendLudo(this);
        currentTile = freeStageTile;
        updatePosition();
    }

    public void MoveOutOfPlay()
    {
        isInPlay = false;
        GetComponent<SpriteRenderer>().enabled = false;
        resetOldTile();
    }

    public void MoveToTile(BoardTile tile)
    {
        resetOldTile();
        appendLudoToNewTile(tile);
        updatePosition();
    }


    private void updatePosition()
    {
        transform.position = currentTile.transform.position;
    }

    private void appendLudoToNewTile(BoardTile tile)
    {
        if (tile.IsOccupied)
            tile.CurrentLudo.KillAndResetUnit();

        tile.AppendLudo(this);
        currentTile = tile;
    }

    private void resetOldTile()
    {
        BoardTile oldTile = currentTile;
        if (oldTile != null)
            oldTile.ResetState();
    }

    public bool IsInPlay()
    {
        return isInPlay;
    }


}
