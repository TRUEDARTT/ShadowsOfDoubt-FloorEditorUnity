using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class FloorSaveData
{
    public string floorName = "newFloor";
    public Vector2 size = new Vector2(1f, 1f);
    public int defaultFloorHeight;
    public int defaultCeilingHeight = 42;
    public List<AddressSaveData> a_d = new List<AddressSaveData>();
    public List<TileSaveData> t_d = new List<TileSaveData>();
}