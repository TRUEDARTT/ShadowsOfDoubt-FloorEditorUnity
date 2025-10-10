using System;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [Range(0, 27)]
    public int selectedDoorWindowPreset = 0;

    public void Reset()
    {
        
    }

    public enum WallModelType
    {
        Wall,
        Window,
        Door,
        Blank
    }

    public static Dictionary<string, string> DoorWindowPresets = new Dictionary<string, string>()
    {
        {"0", "DefaultWalls"},
        {"1", "AlleyBlockWalls"},
        {"2", "Bannister01"},
        {"4", "DividerCentre"},
        {"5", "DividerEndLeft"},
        {"6", "DividerEndRight"},
        {"7", "InteriorDoorway"},
        {"8", "InteriorDoorwayFlat"},
        {"9", "InteriorDoorwayUpper"},
        {"10", "NothingEntrance"},
        {"11", "NothingWall"},
        {"12", "RooftopVentilation"},
        {"13", "RooftopVentilationVent"},
        {"14", "WindowDiner"},
        {"15", "WindowLargeArch"},
        {"16", "WindowLargeRectangle"},
        {"17", "WindowMediumRectangle"},
        {"18", "WindowSmallLower"},
        {"19", "WindowSmallRaised"},
        {"20", "WindowSmallTop"},
        {"21", "WoodenFence"},
        {"22", "WoodenFenceEntrance"},
        {"23", "WoodenFenceJoinLeft"},
        {"24", "WoodenFenceJoinRight"},
        {"25", "DecoHandrail"},
        {"26", "WindowSmallWithUpperSpace"},
        {"27", "WindowSmallLowWithUpperSpace"},
        {"28", "Unknown01" },
        {"29", "Unknown02" },
        {"30", "Unknown03" }
    };
    
    public static Dictionary<string, WallModelType> DoorWindowModels = new Dictionary<string, WallModelType>()
    {
        {"0", WallModelType.Wall},
        {"1", WallModelType.Wall},
        {"2", WallModelType.Wall},
        {"4", WallModelType.Blank},
        {"5", WallModelType.Blank},
        {"6", WallModelType.Blank},
        {"7", WallModelType.Door},
        {"8", WallModelType.Door},
        {"9", WallModelType.Door},
        {"10", WallModelType.Blank},
        {"11", WallModelType.Wall},
        {"12", WallModelType.Wall},
        {"13", WallModelType.Wall},
        {"14", WallModelType.Window},
        {"15", WallModelType.Window},
        {"16", WallModelType.Window},
        {"17", WallModelType.Window},
        {"18", WallModelType.Window},
        {"19", WallModelType.Window},
        {"20", WallModelType.Window},
        {"21", WallModelType.Wall},
        {"22", WallModelType.Door},
        {"23", WallModelType.Wall},
        {"24", WallModelType.Wall},
        {"25", WallModelType.Wall},
        {"26", WallModelType.Window},
        {"27", WallModelType.Window},
        {"28", WallModelType.Wall },
        {"29", WallModelType.Wall },
        {"30", WallModelType.Wall }
    };
}
