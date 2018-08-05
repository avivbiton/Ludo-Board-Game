public class LegalMove
{

    public LudoPiece Ludo;
    public BoardTile AvailableMove;
    public bool CanMoveOutOfPlay;

    public LegalMove(LudoPiece ludo, BoardTile move, bool canMoveOutOfPlay)
    {
        this.Ludo = ludo;
        this.AvailableMove = move;
        this.CanMoveOutOfPlay = canMoveOutOfPlay;
    }
}

