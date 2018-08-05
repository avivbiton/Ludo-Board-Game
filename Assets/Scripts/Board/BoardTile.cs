using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardTile : MonoBehaviour
{

    public static List<BoardTile> AllTiles;
    public static int NormalTileCount = 0;

    [SerializeField]
    private int boardID;
    public bool IsOccupied;
    public LudoPiece CurrentLudo;
    public TileType Type;


    public static BoardTile FindTile(TileType Type, int TileID)
    {
        return AllTiles.First(i => i.Type == Type && i.boardID == TileID);
    }

    private void Start()
    {        
        AllTiles.Add(this);
        if(Type == TileType.Normal)
            NormalTileCount++;
    }


    public int GetID()
    {
        return boardID;
    }

    /// <summary>
    /// Reset the board tile to the default state, where it is not occupied by any piece
    /// </summary>
    public void ResetState()
    {
        IsOccupied = false;
        CurrentLudo = null;
    }

    public void AppendLudo(LudoPiece ludo)
    {
        IsOccupied = true;
        CurrentLudo = ludo;
    }







}
