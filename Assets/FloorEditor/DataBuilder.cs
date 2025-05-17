using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// TODO: AddressVariations, TileData
public class DataBuilder : MonoBehaviour
{
    public TextAsset textAsset;
    public string floorName;

    public void Load()
    {
        var addressManager = GetComponent<AddressManager>();
        var roomManager = GetComponent<RoomManager>();
        var gridManager = GetComponent<GridManager>();

        gridManager.CreateGrid();
        
        addressManager.addresses.Clear();
        roomManager.rooms.Clear();

        var roomTypes = new Dictionary<(string, int), Room>();

        var FloorData = JsonUtility.FromJson<FloorSaveData>(textAsset.text);
        floorName = FloorData.floorName;
        foreach (AddressSaveData address in FloorData.a_d)
        {
            var newAddress = new Address()
            {
                addressPreset = address.p_n,
                color = address.e_c
            };
            
            addressManager.addresses.Add(newAddress);

            if (address.vs.Count > 0)
            {
                AddressLayoutVariation addressVariation = address.vs[0];

                foreach (RoomSaveData room in addressVariation.r_d)
                {
                    if (!roomTypes.ContainsKey((room.l, room.id)))
                    {
                        roomTypes[(room.l, room.id)] = new Room()
                        {
                            id = room.id,
                            roomPreset = room.l,
                            color = Color.white
                        };
                        roomManager.rooms.Add(roomTypes[(room.l, room.id)]);
                    }

                    foreach (NodeSaveData node in room.n_d)
                    {
                        gridManager.GetSquareAt(node.f_c.x, node.f_c.y).AddressPreset = newAddress;
                        gridManager.GetSquareAt(node.f_c.x, node.f_c.y).RoomPreset = roomTypes[(room.l, room.id)];
                        gridManager.GetSquareAt(node.f_c.x, node.f_c.y).NodeSaveData = node;
                        gridManager.GetSquareAt(node.f_c.x, node.f_c.y).UpdateVisuals();
                    }
                }
            }
        }

        foreach (var wall in GetComponentsInChildren<GridWall>(true))
        {
            wall.UpdateVisuals();
        }
    }
    
    public void Save()
    {
        var addressManager = GetComponent<AddressManager>();
        var roomManager = GetComponent<RoomManager>();
        var gridManager = GetComponent<GridManager>();

        FloorSaveData floorData = new FloorSaveData();
        floorData.floorName = floorName;

        var roomSaves = new Dictionary<int, Dictionary<Room, RoomSaveData>>();

        for (var i = 0; i < addressManager.addresses.Count; i++)
        {
            roomSaves[i] = new Dictionary<Room, RoomSaveData>();
        }

        for (int x = 0; x < gridManager.width; x++)
        {
            for (int y = 0; y < gridManager.height; y++)
            {
                var square = gridManager.GetSquareAt(x, y);

                var squareAddressIndex = addressManager.addresses.IndexOf(square.AddressPreset);
                
                if (!roomSaves.ContainsKey(squareAddressIndex))
                    Debug.LogError($"Address not found - {square.AddressPreset.addressPreset} - {squareAddressIndex}!");
                //    roomSaves[square.AddressPreset] = new Dictionary<Room, RoomSaveData>();

                if (!roomSaves[squareAddressIndex].ContainsKey(square.RoomPreset))
                    roomSaves[squareAddressIndex][square.RoomPreset] = new RoomSaveData()
                    {
                        id = square.RoomPreset.id,
                        l = square.RoomPreset.roomPreset,
                        n_d = new List<NodeSaveData>()
                    };

                // Create walls

                roomSaves[squareAddressIndex][square.RoomPreset].n_d.Add(new NodeSaveData()
                {
                    f_c = new Vector2Int(x, y),
                    f_h = square.NodeSaveData.f_h,
                    f_t = square.NodeSaveData.f_t,
                    f_r = "", // Seems to be blank in real files?
                    w_d = square.NodeSaveData.w_d
                });
            }
        }

        for (var i = 0; i < addressManager.addresses.Count; i++)
        {
            var roomSave = roomSaves.ContainsKey(i)
                ? roomSaves[i].Values.ToList()
                : new List<RoomSaveData>();
            var addressLayoutVariations = new List<AddressLayoutVariation>()
            {
                new AddressLayoutVariation()
                {
                    r_d = roomSave
                }
            };

            floorData.a_d.Add(new AddressSaveData()
            {
                p_n = addressManager.addresses[i].addressPreset,
                e_c = addressManager.addresses[i].color,
                vs = addressLayoutVariations
            });
        }

        // TODO: Marking up tile data
        floorData.t_d = JsonUtility.FromJson<FloorSaveData>(textAsset.text).t_d;

        System.IO.File.WriteAllText(@"E:\UnityDev\SodBuildingVisualiser\Assets\FloorSaves\" + floorName + ".json",
            JsonUtility.ToJson(floorData, true));
        AssetDatabase.Refresh();
        Debug.Log("Saved");
    }
}