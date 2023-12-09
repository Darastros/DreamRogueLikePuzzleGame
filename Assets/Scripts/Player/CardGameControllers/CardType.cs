using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [Flags]
    public enum CardType
    {
        WOOD = 1,
        STONE = 2,
        FIRE = 4,
        IRON = 8,
        GOLD = 16
    }
}