using System;
using System.Collections.Generic;
using System.Linq;
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

        addressManager.addresses.Clear();
        roomManager.rooms.Clear();

        var roomTypes = new Dictionary<(string, int), Room>();

        var FloorData = JsonUtility.FromJson<FloorSaveData>(textAsset.text);
        floorName = FloorData.floorName;
        foreach (AddressSaveData address in FloorData.a_d)
        {
            addressManager.addresses.Add(new Address()
            {
                addressPreset = address.p_n,
                color = address.e_c
            });

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
                        gridManager.GetSquareAt(node.f_c.x, node.f_c.y)
                            .SetAddressPreset(addressManager.addresses.Last());
                        gridManager.GetSquareAt(node.f_c.x, node.f_c.y).SetRoomType(roomTypes[(room.l, room.id)]);
                        gridManager.GetSquareAt(node.f_c.x, node.f_c.y).NodeSaveData = node;
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

        var nodeSaves = new Dictionary<Address, List<NodeSaveData>>();
        var roomSaves = new Dictionary<Address, Dictionary<Room, RoomSaveData>>();

        for (int x = 0; x < gridManager.width; x++)
        {
            for (int y = 0; y < gridManager.height; y++)
            {
                var square = gridManager.GetSquareAt(x, y);

                if (!nodeSaves.ContainsKey(square.AddressPreset))
                    nodeSaves[square.AddressPreset] = new List<NodeSaveData>();

                if (!roomSaves.ContainsKey(square.AddressPreset))
                    roomSaves[square.AddressPreset] = new Dictionary<Room, RoomSaveData>();

                if (!roomSaves[square.AddressPreset].ContainsKey(square.RoomPreset))
                    roomSaves[square.AddressPreset][square.RoomPreset] = new RoomSaveData()
                    {
                        id = square.RoomPreset.id,
                        l = square.RoomPreset.roomPreset,
                        n_d = new List<NodeSaveData>()
                    };

                // Create walls

                roomSaves[square.AddressPreset][square.RoomPreset].n_d.Add(new NodeSaveData()
                {
                    f_c = new Vector2Int(x, y),
                    f_h = square.NodeSaveData.f_h,
                    f_t = square.NodeSaveData.f_t,
                    f_r = "", // Seems to be blank in real files?
                    w_d = square.NodeSaveData.w_d
                });
            }
        }

        foreach (var address in addressManager.addresses)
        {
            var addressLayoutVariations = new List<AddressLayoutVariation>()
            {
                new AddressLayoutVariation()
                {
                    r_d = roomSaves.ContainsKey(address) ? roomSaves[address].Values.ToList() : new List<RoomSaveData>()
                }
            };

            floorData.a_d.Add(new AddressSaveData()
            {
                p_n = address.addressPreset,
                e_c = address.color,
                vs = addressLayoutVariations
            });
        }

        // TODO: Marking up tile data
        floorData.t_d = JsonUtility.FromJson<FloorSaveData>(textAsset.text).t_d;

        System.IO.File.WriteAllText(@"E:\UnityDev\SodBuildingVisualiser\Assets\FloorSaves\testNewBuilding.json",
            JsonUtility.ToJson(floorData, true));
        Debug.Log("Saved");
    }
}