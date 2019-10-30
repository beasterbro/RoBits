using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayTimer
{
    private readonly float maxDelay;
    private float currentDelay;

    // Construct a DelayTimer initialized with HasTimePassed() == false
    public DelayTimer(float maxDelay) : this(maxDelay, false) { }

    // Construct a DelayTimer that either starts in the state of IsTimeUp() == true, or HasTimePassed() == false
    public DelayTimer(float maxDelay, bool isTimeUp)
    {
        this.maxDelay = maxDelay;
        Reset();
        if (isTimeUp)
        {
            Increment(maxDelay);
        }
    }
    
    // Return maximum delay timer can reach.
    public float GetDelay()
    {
        return maxDelay;
    }

    // Return current time elapsed. Locked between [0-GetDelay()].
    public float GetTime()
    {
        return currentDelay;
    }

    public bool IsTimeUp()
    {
        return maxDelay <= currentDelay;
    }

    private bool HasTimePassed()
    {
        return currentDelay <= 0.0f;
    }

    public void Reset()
    {
        currentDelay = 0.0f;
    }

    public void Increment(float delta)
    {
        currentDelay = Mathf.Clamp(currentDelay + delta, 0f, maxDelay);
    }

    public void Decrement(float delta)
    {
        this.Increment(-delta);
    }
}
