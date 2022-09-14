﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MCHexBOT.Utils.Data
{
    public enum DiggingStatus
    {
        Started = 0,
        Cancelled = 1,
        Finished = 2,
        DropItemStack = 3,
        DropItem = 4,
        ShootArrow = 5,
        FinishEating = 5,
        SwapItemInHand = 6
    }
}
