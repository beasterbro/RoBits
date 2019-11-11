using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LoadingTicker : MonoBehaviour
{
    [SerializeField] private float delay;
    private DelayTimer timer;

    protected abstract void Tick();

    // Start is called before the first frame update
    void Start()
    {
        timer = new DelayTimer(delay);
    }

    // Update is called once per frame
    void Update()
    {
        timer.Increment(Time.deltaTime);
        if (timer.IsTimeUp())
        {
            timer.Reset();
            Tick();
        }
    }
}
