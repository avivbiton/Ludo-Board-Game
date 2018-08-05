using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class TextComponent : DeployableUI
{

    [SerializeField]
    private GameObject textPrefab;

    public string textToDisplay;

    private GameObject currentObject;

    public override void Deploy()
    {
        currentObject = Instantiate(textPrefab, transform);
        currentObject.GetComponent<TextMeshProUGUI>().text = textToDisplay;
        AnimationWait waiter = currentObject.GetComponent<AnimationWait>();

        waiter.RegisterCallBack(animEnded);
    }

    private void animEnded()
    {
        Destroy(currentObject);
        EndDepoyable();
        Destroy(gameObject);
    }




}

