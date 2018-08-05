using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeployableUI : MonoBehaviour
{

    private Action callbackListener;

    public void RegisterListener(Action listener)
    {
        callbackListener += listener;
    }

    public void UnRegisterListener(Action listener)
    {
        callbackListener -= listener;
    }

    protected void EndDepoyable()
    {
        if (callbackListener != null)
            callbackListener();
    }

    public abstract void Deploy();


}

