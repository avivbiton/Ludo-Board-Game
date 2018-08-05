
public interface IGameInputReceiver
{
    void InputMove(Player player, LudoPiece piece, BoardTile tile);

    void InputPutLudoOutOfPlay(Player player, LudoPiece piece);
}

