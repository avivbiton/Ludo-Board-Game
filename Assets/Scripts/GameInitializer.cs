using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Initialize all the components needed for the game to run and their settings.
/// </summary>
public class GameInitializer : MonoBehaviour
{

    private const int LUDO_PIECIES_PER_PLAYER = 4;

    /// <summary>
    /// Pre-defined tiles for each colored player, contains data about starting, staged and home tiles.
    /// This should be set up in the inspector.
    /// </summary>
    [SerializeField]
    private BoardHomeTiles[] PredefinedTiles;
    [SerializeField]
    private Sprite ludoSprite;
    [SerializeField]
    private Color[] ludoColors;

    private Player[] currentPlayers;
    private int playerCount;

    private void Start()
    {
        BoardTile.AllTiles = new List<BoardTile>();
        initializeGameFor(4);
    }

    private void initializeGameFor(int playerCount)
    {
        this.playerCount = playerCount;
        Debug.Log("Initalizing game for " + playerCount + " players.");
        initalizePlayers();
        initalizeAndStartGameRound();
        destoryInitializer();
    }

    /// <summary>
    /// initalize the player with their predefined home tiles
    /// </summary>
    private void initalizePlayers()
    {
        currentPlayers = new Player[playerCount];
        for (int PLAYER_ID = 0; PLAYER_ID < playerCount; PLAYER_ID++)
            currentPlayers[PLAYER_ID] = initalizePlayer(PLAYER_ID, PredefinedTiles[PLAYER_ID]);
    }


    private void initalizeAndStartGameRound()
    {
        GameObject gameRoundObject = new GameObject("GameRoundManager", typeof(GameRound));
        GameRound round = gameRoundObject.GetComponent<GameRound>();

        round.SetupPlayers(currentPlayers);
        round.StartRound();
    }

    private Player initalizePlayer(int player_id, BoardHomeTiles tiles)
    {
        Player player = new Player(player_id, tiles, ludoColors[player_id]);
        createBoardPiecesForPlayer(player);
        defineTypeForBoardTiles(getHomeTilesForPlayerId(player_id));
        return player;
    }

    private void createBoardPiecesForPlayer(Player player)
    {
        LudoPiece[] playerLudos = new LudoPiece[LUDO_PIECIES_PER_PLAYER];
        BoardHomeTiles homeTiles = getHomeTilesForPlayerId(player.GetPlayerID());

        for (int i = 0; i < LUDO_PIECIES_PER_PLAYER; i++)
        {
            playerLudos[i] = LudoPiece.CreateLudoWithGameobject(startingTile: homeTiles.StagedTiles[i]);
            playerLudos[i].SetVisualApperance(ludoSprite, ludoColors[player.GetPlayerID()]);
        }

        player.SetupLudoPieces(playerLudos);
    }

    private BoardHomeTiles getHomeTilesForPlayerId(int playerId)
    {
        return PredefinedTiles[playerId];
    }

    private void defineTypeForBoardTiles(BoardHomeTiles tiles)
    {
        foreach (BoardTile t in tiles.StagedTiles)
            t.Type = TileType.Stage;

        foreach (BoardTile t in tiles.HomeTiles)
            t.Type = TileType.Home;
    }

    private void destoryInitializer()
    {
        // we just destory for now, might want to add something else later, like firing an event with the initalizer is done.
        Destroy(this);
    }



}
