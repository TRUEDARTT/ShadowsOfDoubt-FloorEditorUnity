using System;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    private GridManager gridManager;
    public GridSquare SelectedNode;
    
    public NewNode.FloorTileType SelectedFloorTileType;

    [Range(0, 10)]
    public int ExtraFloorHeight = 0;

    public void Reset()
    {
        
    }
}
