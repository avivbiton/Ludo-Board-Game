using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollDiceComponent : DeployableUI
{

    public GameObject buttonPrefab;
    public GameObject textPrefab;

    private GameObject buttonObject;
    private GameObject textObject;

    [HideInInspector]
    public int NumberToDisplay;
  
    public override void Deploy()
    {
        buttonObject = Instantiate(buttonPrefab, transform);
        Button buttonScript = buttonObject.GetComponent<Button>();
        buttonScript.onClick.AddListener(onButtonClicked);
    
    }

    private void onButtonClicked()
    {
        Destroy(buttonObject);
        textObject = Instantiate(textPrefab, transform);
        string text = "Dice Rolled " + NumberToDisplay;
        textObject.GetComponent<TextMeshProUGUI>().text = text;

        AnimationWait waiter = textObject.GetComponent<AnimationWait>();
        waiter.RegisterCallBack(finishDeployable);

    }



    private void finishDeployable()
    {
        Destroy(textObject);
        Destroy(gameObject);
        EndDepoyable();
    }



}

