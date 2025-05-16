using System;
using UnityEngine;

[Serializable]
public class TileSaveData
{
    // floorCoord
    // Multiplied by 7 to get the real value (From CityControls.tileMultiplier)
    public Vector2Int f_c;

    // isEntrance
    public bool i_e;

    // isMainEntrance
    public bool m_e;

    // isStairwell
    public bool s_t;

    // isInvertedStairwell
    public bool e_l;
    
    // stairwellRotation
    public int s_r;

    // elevatorRotation
    public int e_r;
}
