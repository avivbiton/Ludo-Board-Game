using System;

public class DiceRollEvent : Event<DiceRollEvent>
{
    public Dice diceUsed;
    public int result;
}



