using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class NodeSaveData
{
    // Floor coordinates/position
    // Multiplied by [1.8, 1.8, 5.4] to get the real position (From Pathfinding.nodeSize)
    public Vector2Int f_c;    
    public int f_h;          // Floor height
    public NewNode.FloorTileType f_t;  // Floor type
    public string f_r;       // Floor reference/id
    public List<WallSaveData> w_d;    // Wall data
}


public class NewNode
{
    public enum FloorTileType
    {
        none,
        floorAndCeiling,
        floorOnly,
        CeilingOnly,
        noneButIndoors,
    }
}
