using UnityEngine;

public class Dice
{

    private int maxResult = 6;
    private int currentResult;

    public int Roll()
    {
        currentResult = Random.Range(1, maxResult + 1);
        fireDiceEvent();
        return currentResult;
    }

    public int GetDiceResult()
    {
        return currentResult;
    }

    private void fireDiceEvent()
    {
        DiceRollEvent eventRoll = new DiceRollEvent();
        eventRoll.diceUsed = this;
        eventRoll.result = currentResult;
        eventRoll.FireEvent();
    }

}

