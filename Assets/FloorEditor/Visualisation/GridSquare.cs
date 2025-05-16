using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;


public class GridSquare : MonoBehaviour
{
    private Address _addressPreset;
    private Room _roomPreset;
    public NodeSaveData NodeSaveData = new NodeSaveData();
    
    public Color color = Color.white;
    public bool IsLocked = false;

    public Material MaterialToUse;
    private Material thisMat;

    [SerializeField]
    private MeshRenderer quadMeshRenderer;
    [SerializeField]
    private TextMeshPro labelTMP;
    
    public static Address NullAddress = new Address()
    {
        addressPreset = "Null",
        color = Color.magenta
    };

    public static Room NullRoom = new Room()
    {
        roomPreset = "Null",
        color = Color.magenta
    };
    
    private void OnValidate()
    {
        if (!thisMat)
        {
            thisMat = new Material(MaterialToUse);
            quadMeshRenderer.sharedMaterial = thisMat;
        }
        
        UpdateVisuals();
    }

    private void Reset()
    {
        UpdateVisuals();
    }

    public Address AddressPreset => _addressPreset ?? NullAddress;
    
    public void SetAddressPreset(Address addressPreset)
    {
        if (!IsLocked)
        {
            _addressPreset = addressPreset;
            UpdateVisuals();
        }
    }

    public Room RoomPreset => _roomPreset ?? null;
    
    public void SetRoomType(Room roomPreset)
    {
        _roomPreset = IsLocked ? NullRoom : roomPreset;
        // You can add specific color mappings for room types here if desired
        UpdateVisuals();
    }
    
    public void UpdateVisuals()
    {
        var color = Color.white;
        if (IsLocked) color = Color.gray;
        else if (_addressPreset != null) color = _addressPreset.color;
        
        thisMat.SetColor("_BaseColor", color);

        if (_roomPreset != null)
        {
            labelTMP.text = $"{_roomPreset.roomPreset} - {_roomPreset.id}";
            labelTMP.color = _roomPreset.color;
        }
    }

    public void OnDrawGizmos()
    {
        if(NodeSaveData.f_t is NewNode.FloorTileType.floorAndCeiling or NewNode.FloorTileType.floorOnly)
            Gizmos.DrawWireCube(transform.position + new Vector3(0, (NodeSaveData.f_h / 10f) + -0.05f, 0), new Vector3(1, 0.1f, 1));
        
        if(NodeSaveData.f_t is NewNode.FloorTileType.floorAndCeiling or NewNode.FloorTileType.CeilingOnly)
            Gizmos.DrawWireCube(transform.position + new Vector3(0, 5.45f, 0), new Vector3(1, 0.1f, 1));
    }
}