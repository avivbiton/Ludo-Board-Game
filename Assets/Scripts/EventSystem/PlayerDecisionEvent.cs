using System.Collections.Generic;

public class PlayerDecisionEvent : Event<PlayerDecisionEvent>
{
    public Player player;
    public List<LegalMove> legalMoves;

}



