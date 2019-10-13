using System.Collections.Generic;
using UnityEngine;

// TODO: Add custom serializer
public class BotInfo
{

    private class DbBot
    {
        public int bid;
        public string name;
        public int[] parts;
        public int tier;
        public Dictionary<string, string> ai; // TODO: Change to a better data type later
    }

    
    
    // botBehavior
    // equippedParts
    // bodyType

    // GetEquippedParts
    // AddPart
    // RemovePart
    // GetBotBehavior
    // SetCodeBlocks
    // IsCpuLimitReached
    // IsMaxPartsReached

}
