using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Don't know if this is the best way to do this. TODO: Should look for more effective way.
public class BehaviorData
{
    public static readonly BehaviorData EMPTY = new BehaviorData();

    private readonly ReturnType type;
    private readonly bool logical;
    private readonly float number;
    private readonly BotController bot;

    private BehaviorData()
    {
        this.type = ReturnType.EMPTY;
        this.logical = false;
        this.number = 0;
        this.bot = null;
    }

    public BehaviorData(bool logical)
    {
        this.type = ReturnType.LOGICAL;
        this.logical = logical;
        this.number = this.logical ? 1 : 0;
        this.bot = null;
    }

    public BehaviorData(float number)
    {
        this.type = ReturnType.NUMBER;
        this.number = number;
        this.logical = this.number != 0;
        this.bot = null;
    }

    public BehaviorData(BotController bot)
    {
        this.type = ReturnType.BOT;
        this.bot = bot;
        this.logical = this.bot != null;
        this.number = 0;
    }

    public ReturnType GetReturnType()
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

    public bool IsBot()
    {
        return IsEqualType(ReturnType.BOT);
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

    public BotController GetBot()
    {
        VerifyContent();
        return this.bot;
    }
}
