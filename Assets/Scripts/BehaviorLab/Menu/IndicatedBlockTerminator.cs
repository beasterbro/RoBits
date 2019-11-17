using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatedBlockTerminator : BlockTerminator
{
    [SerializeField] private GameObject indicator;

    protected override void OnGrab()
    {
        // do nothing
    }

    protected override void OnOver()
    {
        indicator.SetActive(true);
    }

    protected override void OnExit()
    {
        indicator.SetActive(false);
    }
}
