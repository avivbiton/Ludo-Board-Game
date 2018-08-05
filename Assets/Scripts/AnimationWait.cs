using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationWait : MonoBehaviour
{

    public bool Ended = false;
    public Action Callback;

    public void RegisterCallBack(Action call)
    {
        Callback = call;
    }

    private void onEnd()
    {
        Ended = true;
        if (Callback != null)
            Callback();
    }

}
