using System;
using System.Linq;

[Serializable]
public class PrivateBoardTiles
{


    /// <summary>
    /// Data class that stores the private tiles for each player.
    /// </summary>

    public BoardTile[] StagedTiles;
    public BoardTile[] HomeTiles;
    public BoardTile StartingTile;

    public BoardTile FindEmptyStageBoardTile()
    {
        return StagedTiles.FirstOrDefault(i => i.Type == TileType.Stage && i.IsOccupied == false);
    }

    public BoardTile GetStartingTile()
    {
        return StartingTile;
    }

    public BoardTile FindHomeDestinationTile(int homeID)
    {
        return HomeTiles.First(i => i.GetID() == homeID);
    }

    /// <summary>
    /// Returns the tile the player is needed to reach in order to enter their home tile
    /// </summary>
    /// <returns></returns>
    public BoardTile GetEndPointTile()
    {
        int id = StartingTile.GetID() - 2;
        return BoardTile.FindTile(TileType.Normal, id);
    }
}

