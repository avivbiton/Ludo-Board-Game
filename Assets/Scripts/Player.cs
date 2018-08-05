using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player
{

    private int playerID;
    private LudoPiece[] ludosOwned;
    private Color boardColor;


    /// <summary>
    /// The predefined staged, home and starting tile.
    /// Each colored player has a different set of tiles data
    /// </summary>
    private BoardHomeTiles playerBoardTiles;

    public Player(int playerID, BoardHomeTiles playerBoardTiles, Color color)
    {
        this.playerID = playerID;
        this.playerBoardTiles = playerBoardTiles;
        this.boardColor = color;
    }

    public int GetPlayerID()
    {
        return playerID;
    }

    public LudoPiece[] GetPlayerLudos()
    {
        return ludosOwned;
    }

    public void SetupLudoPieces(LudoPiece[] ludoPieces)
    {
        ludosOwned = ludoPieces;
        foreach (LudoPiece ludo in ludosOwned)
            ludo.GrantOwnershipTo(this);
    }

    public bool IsTileOwnerOf(BoardTile tile)
    {
        foreach (BoardTile t in playerBoardTiles.HomeTiles)
            if (t == tile)
                return true;
        return false;
    }

    public BoardTile FindEmptyStageBoardTile()
    {
        return playerBoardTiles.StagedTiles.FirstOrDefault(i => i.Type == TileType.Stage && i.IsOccupied == false);
    }

    public BoardTile GetStartingTile()
    {
        return playerBoardTiles.StartingTile;
    }

    /// <summary>
    /// Returns the tile the player is needed to reach in order to enter their home tile
    /// </summary>
    /// <returns></returns>
    public BoardTile GetEndPointTile()
    {
        int id = playerBoardTiles.StartingTile.GetID() - 2;
        return BoardTile.FindTile(TileType.Normal, id);
    }

    public Color GetColor()
    {
        return boardColor;
    }


}

