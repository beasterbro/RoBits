using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTextTicker : LoadingTicker
{
    [SerializeField] private Text text;
    private int tick = 0;
    private int maxTicks = 3;

    protected override void Tick()
    {
        tick = (tick + 1) % (maxTicks + 1);
        text.text = "Loading" + new string('.', tick);
    }
}
