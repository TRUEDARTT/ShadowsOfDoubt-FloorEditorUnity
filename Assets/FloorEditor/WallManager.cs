using System;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [Range(0, 27)]
    public int selectedDoorWindowPreset = 0;
    
    private void OnValidate()
    {
        // currentDoorWindowPresetName = DoorWindowPresets[selectedDoorWindowPreset.ToString()];
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
        {"27", "WindowSmallLowWithUpperSpace"}
    };
}
