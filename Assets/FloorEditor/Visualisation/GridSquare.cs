using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;

// TODO: Fix locked
public class GridSquare : MonoBehaviour
{
    public Address AddressPreset;
    public Room RoomPreset;
    public NodeSaveData NodeSaveData = new NodeSaveData();
    
    public Color color = Color.white;
    public bool IsLocked = false;

    public Material MaterialToUse;
    private Material thisMat;

    [SerializeField]
    private MeshRenderer quadMeshRenderer;
    [SerializeField]
    private TextMeshPro labelTMP;
    
    
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
    
    public void UpdateVisuals()
    {
        var color = Color.white;
        if (IsLocked) color = Color.gray;
        else if (AddressPreset != null) color = AddressPreset.color;
        
        thisMat.SetColor("_BaseColor", color);

        if (RoomPreset != null)
        {
            labelTMP.text = $"{RoomPreset.roomPreset} - {RoomPreset.id}\n({NodeSaveData.f_c.x},{NodeSaveData.f_c.y})";
            labelTMP.color = RoomPreset.color;
        }
    }
}