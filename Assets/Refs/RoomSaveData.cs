using System.Collections.Generic;
using System;

[Serializable]
public class RoomSaveData
{
    public int id;
    public List<NodeSaveData> n_d = new List<NodeSaveData>();
    public string l; // RoomTypePreset string reference
}
