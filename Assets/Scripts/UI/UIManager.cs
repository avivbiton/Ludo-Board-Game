using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {


    public TextComponent TextComp;
    public RollDiceComponent RollDiceComp;
    public TileHighlightComponent HighlightComp;
    

    private UIQueue uiQueue;


    public void Awake()
    {
        uiQueue = new UIQueue();

        registerEvents();
    }

    private void registerEvents()
    {
        StartOfTurnEvent.RegisterListener(onStartOfTurn);
        DiceRollEvent.RegisterListener(onDiceRoll);
        PlayerDecisionEvent.RegisterListener(onPlayerAction);
        PlayerWonEvent.RegisterListener(onPlayerWon);
      
    }

    private void onStartOfTurn(StartOfTurnEvent data)
    {
        
        string text = "<#" + ColorUtility.ToHtmlStringRGB(data.player.GetPlayerColor()) + ">Player (" + data.player.GetPlayerID() + ")'s Turn ! </color>";
        var component = Instantiate(TextComp, transform);
        component.GetComponent<TextComponent>().textToDisplay = text;
        uiQueue.Enqueue(component);

    }

    private void onDiceRoll(DiceRollEvent data)
    {
        var component = Instantiate(RollDiceComp, transform) as RollDiceComponent;
        component.NumberToDisplay = data.diceUsed.GetDiceResult();
        uiQueue.Enqueue(component);

    }

    private void onPlayerAction(PlayerDecisionEvent data)
    {
        var component = Instantiate(HighlightComp, transform) as TileHighlightComponent;
        component.Data = data;
        uiQueue.Enqueue(component);
    }

    private void onPlayerWon(PlayerWonEvent data)
    {

    }



}
