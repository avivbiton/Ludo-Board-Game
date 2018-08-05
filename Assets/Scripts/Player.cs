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
    private PrivateBoardTiles playerBoardTiles;

    public Player(int playerID, PrivateBoardTiles playerBoardTiles, Color color)
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

    public PrivateBoardTiles GetPlayerBoardTiles()
    {
        return playerBoardTiles;
    }

    public Color GetPlayerColor()
    {
        return boardColor;
    }


}

