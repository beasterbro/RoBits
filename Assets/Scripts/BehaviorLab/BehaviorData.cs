using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Don't know if this is the best way to do this. TODO: Should look for more effective way.
public class BehaviorData
{
    public static readonly BehaviorData EMPTY = new BehaviorData();

    private ReturnType type;
    private bool logical;
    private float number;

    private BehaviorData()
    {
        this.type = ReturnType.EMPTY;
        this.logical = false;
        this.number = 0;
    }

    public BehaviorData(bool logical)
    {
        this.type = ReturnType.LOGICAL;
        this.logical = logical;
        this.number = this.logical ? 1 : 0;
    }

    public BehaviorData(float number)
    {
        this.type = ReturnType.NUMBER;
        this.number = number;
        this.logical = this.number != 0;
    }

    public ReturnType GetType()
    {
        return type;
    }

    private bool IsEqualType(ReturnType other)
    {
        return this.type == other;
    }

    public bool IsLogical()
    {
        return IsEqualType(ReturnType.LOGICAL);
    }

    public bool IsNumber()
    {
        return IsEqualType(ReturnType.NUMBER);
    }

    public bool IsBlank()
    {
        return IsEqualType(ReturnType.EMPTY);
    }

    private void VerifyContent()
    {
        if (IsBlank())
        {
            throw new InvalidOperationException("Data was empty.");
        }
    }

    public bool GetLogical()
    {
        VerifyContent();
        return this.logical;
    }

    public float GetNumber()
    {
        VerifyContent();
        return this.number;
    }
}
