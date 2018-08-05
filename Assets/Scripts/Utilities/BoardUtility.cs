using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class BoardUtility
{
    public static List<BoardTile> GetTilesBetween(BoardTile startTile, BoardTile endTile)
    {
        List<BoardTile> tiles = new List<BoardTile>();
        BoardTile current = FindDestinationTile(CorrectTileIdIfInvalid(startTile.GetID() + 1));
        while (current != endTile)
        {
            tiles.Add(current);
            current = FindDestinationTile(CorrectTileIdIfInvalid(current.GetID() + 1));
        }

        return tiles;
    }

    public static bool IsPathBlocked(List<BoardTile> tilesToCheck)
    {
        return tilesToCheck.Any(i => i.IsOccupied);
    }

    public static BoardTile FindDestinationTile(int tileID)
    {
        return BoardTile.AllTiles.First(i => i.GetID() == tileID);
    }


    public static int CorrectTileIdIfInvalid(int destinationTileId)
    {
        if (destinationTileId >= BoardTile.NormalTileCount)
        {
            int difference = destinationTileId - BoardTile.NormalTileCount;
            destinationTileId = 0 + difference;
        }

        return destinationTileId;
    }
}



