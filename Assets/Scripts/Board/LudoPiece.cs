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

    public BoardTile GetLegalMoveForRoll(int roll)
    {
        if (currentTile.Type == TileType.Stage)
            return getLegalMovesForStagedTiles(roll);
        else if (currentTile.Type == TileType.Home)
            return getLegalMovesForHomeTiles(roll);
        else if (currentTile.Type == TileType.Normal)
            return getLegalMoveForNormalTiles(roll);


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

    private BoardTile getLegalMovesForStagedTiles(int roll)
    {
        if (roll != 6) return null;

        BoardTile destinationTile = owner.GetStartingTile();
        if (destinationTile.IsOccupied && destinationTile.CurrentLudo.owner == owner) return null;

        return destinationTile;
    }

    private BoardTile getLegalMovesForHomeTiles(int roll)
    {
        // If the unit can already move out of play, skip this, we inform the UI about this in other way.
        if (CanMoveOutOfPlay(roll)) return null;

        int currentStage = currentTile.GetID();
        int requiredRoll = currentStage + 1;
        if (roll != requiredRoll)
            return null;
        BoardTile destinationTile = findHomeDestinationTile(requiredRoll);
        if (destinationTile.IsOccupied)
            return null;
        return destinationTile;
    }

    private BoardTile getLegalMoveForNormalTiles(int roll)
    {
        if (currentTile == owner.GetEndPointTile())
            return getHomeTileIfAvailable(roll);

        int destinationTileId = currentTile.GetID() + roll;
        destinationTileId = validateAndGetTileId(destinationTileId);
        BoardTile destinationTile = findDestinationTile(destinationTileId);

        List<BoardTile> tilesBetween = getBoardTilesBetween(currentTile, destinationTile);
        if (isPathBlocked(tilesBetween))
            return null;

        if (tilesBetween.Contains(owner.GetEndPointTile()))
            return null;


        if (destinationTile.IsOccupied && destinationTile.CurrentLudo.owner == owner) return null;
        return destinationTile;
    }

    /// <summary>
    /// returns the board tile when just climbing into the home tile
    /// </summary>
    /// <param name="roll"></param>
    /// <returns></returns>
    private BoardTile getHomeTileIfAvailable(int roll)
    {
        if (roll == 1)
            return findHomeDestinationTile(1);
        return null;

    }

    /// <summary>
    /// Reset and then set the tile id to the correct one if tile id is over the NormalTileCount
    /// </summary>>
    private int validateAndGetTileId(int destinationTileId)
    {
        if (destinationTileId >= BoardTile.NormalTileCount)
        {
            int difference = destinationTileId - BoardTile.NormalTileCount;
            destinationTileId = 0 + difference;
        }

        return destinationTileId;
    }

    private List<BoardTile> getBoardTilesBetween(BoardTile startTile, BoardTile endTile)
    {
        List<BoardTile> tiles = new List<BoardTile>();
        BoardTile current = findDestinationTile(validateAndGetTileId(startTile.GetID() + 1));
        while (current != endTile)
        {
            tiles.Add(current);
            current = findDestinationTile(validateAndGetTileId(current.GetID() + 1));
        }

        return tiles;
    }

    private bool isPathBlocked(List<BoardTile> tilesToCheck)
    {
        return tilesToCheck.Any(i => i.IsOccupied);
    }

    private BoardTile findDestinationTile(int tileID)
    {
        return BoardTile.AllTiles.First(i => i.GetID() == tileID);
    }

    private BoardTile findHomeDestinationTile(int homeID)
    {
        return BoardTile.AllTiles.First(i => i.GetID() == homeID && owner.IsTileOwnerOf(i));
    }

    /// <summary>
    /// Kills the unit and reset it back to staged tile. called when it is "eaten" by other unit.
    /// </summary>
    public void KillAndResetUnit()
    {
        BoardTile freeStageTile = owner.FindEmptyStageBoardTile();
        if (freeStageTile == null)
            throw new ArgumentException("could not find a free stage tile.");
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
