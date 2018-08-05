using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIQueue
{


    private Queue<DeployableUI> deployables;

    public UIQueue()
    {
        deployables = new Queue<DeployableUI>();
    }


    public void Enqueue(DeployableUI deployable)
    {       
        deployable.RegisterListener(onDeployableEnded);
        deployables.Enqueue(deployable);

        if (deployables.Count == 1)
            deployNext();

    }

    private void deployNext()
    {
        if (deployables.Count != 0)
            deployables.Peek().Deploy();
    }

    private void onDeployableEnded()
    {
        deployables.Dequeue();
        deployNext();
    }
}

