using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class Clickable : MonoBehaviour {


    private Action listeners;

    public void AddListener(Action listener)
    {
        listeners += listener;
    }

    public void RemoveListener(Action listener)
    {
        listeners -= listener;
    }

    private void OnMouseUp()
    {
        if (listeners != null)
            listeners();
    }

}
