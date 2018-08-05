using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TileHighlightComponent : DeployableUI
{
    public PlayerDecisionEvent Data;

    [SerializeField]
    private GameObject highlightObjectPrefab;
    private List<LudoPiece> moveableUnits;
    private List<GameObject> highlightObjectsCreated;
    private LudoPiece selectedLudo = null;


    public override void Deploy()
    {
        highlightObjectsCreated = new List<GameObject>();
        highlightMoveableUnits();
    }

    private void highlightMoveableUnits()
    {
        getUnitsWithAvailableMoves();

        foreach (LudoPiece ludo in moveableUnits)
        {
            GameObject highlightTile = createHighlightObject();
            highlightTile.transform.position = ludo.transform.position;
            highlightTile.GetComponent<Clickable>().AddListener(() =>
            {
                Debug.Log("On click");
                onLudoSelected(ludo);
            });
        }

    }

    private void getUnitsWithAvailableMoves()
    {
        moveableUnits = new List<LudoPiece>();
        foreach (LegalMove move in Data.legalMoves)
        {
            if (move.AvailableMove != null || move.CanMoveOutOfPlay != false)
                moveableUnits.Add(move.Ludo);
        }
    }

    private GameObject createHighlightObject()
    {
        GameObject highlightObject = Instantiate(highlightObjectPrefab, transform);
        highlightObjectsCreated.Add(highlightObject);
        return highlightObject;
    }


    private void onLudoSelected(LudoPiece ludo)
    {
        if (selectedLudo != null)
            clearPreviousHighlight();

        selectedLudo = ludo;
        LegalMove legalMove = getLegalMoveForLudo(ludo);
        if (legalMove.CanMoveOutOfPlay)
            sendMoveOutOfPlay();
        else
        {
            GameObject highlight = createHighlightObject();
            highlight.transform.position = legalMove.AvailableMove.transform.position;
            highlight.GetComponent<Clickable>().AddListener(() =>
            {
                sendMove(legalMove.AvailableMove);
            });
        }



    }

    private void clearPreviousHighlight()
    {
        GameObject highlight = highlightObjectsCreated.Last();
        Destroy(highlight);
        highlightObjectsCreated.Remove(highlight);
    }

    private LegalMove getLegalMoveForLudo(LudoPiece ludo)
    {
        return Data.legalMoves.FirstOrDefault(i => i.Ludo == ludo);
    }

    private void sendMoveOutOfPlay()
    {
        IGameInputReceiver inputReceiver = GameRound.GetInputReceiver();
        inputReceiver.InputPutLudoOutOfPlay(Data.player, selectedLudo);

        finishDeployable();
    }

    private void sendMove(BoardTile tile)
    {
        IGameInputReceiver inputReceiver = GameRound.GetInputReceiver();
        inputReceiver.InputMove(Data.player, selectedLudo, tile);

        finishDeployable();
    }




    private void finishDeployable()
    {
        foreach (GameObject obj in highlightObjectsCreated)
            Destroy(obj);

        Destroy(gameObject);
        EndDepoyable();
    }



}
