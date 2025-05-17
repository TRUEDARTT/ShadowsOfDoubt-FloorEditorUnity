using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeManager))]
public class NodeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NodeManager nodeManager = (NodeManager)target;
        
        EditorGUILayout.LabelField("Paint Settings", EditorStyles.boldLabel);
        
        nodeManager.SelectedFloorTileType = (NewNode.FloorTileType)EditorGUILayout.EnumPopup("Tile Type", nodeManager.SelectedFloorTileType);
        
        EditorGUILayout.Space(10);

        // If nodeData is null, don't try to draw its properties
        if (!nodeManager.SelectedNode || nodeManager.SelectedNode.NodeSaveData == null)
            return;

        EditorGUILayout.LabelField("Node Data", EditorStyles.boldLabel);
        
        // Floor Position
        EditorGUI.indentLevel++;
        EditorGUILayout.LabelField("Floor Position", EditorStyles.boldLabel);
        nodeManager.SelectedNode.NodeSaveData.f_c = EditorGUILayout.Vector2IntField("Coordinates", nodeManager.SelectedNode.NodeSaveData.f_c);
        nodeManager.SelectedNode.NodeSaveData.f_h = EditorGUILayout.IntField("Height", nodeManager.SelectedNode.NodeSaveData.f_h);
        
        // Floor Properties
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Floor Properties", EditorStyles.boldLabel);
        nodeManager.SelectedNode.NodeSaveData.f_t = (NewNode.FloorTileType)EditorGUILayout.EnumPopup("Tile Type", nodeManager.SelectedNode.NodeSaveData.f_t);
        nodeManager.SelectedNode.NodeSaveData.f_r = EditorGUILayout.TextField("Reference ID", nodeManager.SelectedNode.NodeSaveData.f_r);
        
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Walls", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("-x", nodeManager.SelectedNode.NodeSaveData.w_d.FirstOrDefault(wd => wd.w_o.x < 0)?.p_n ?? "Nothing");
        EditorGUILayout.LabelField("+x", nodeManager.SelectedNode.NodeSaveData.w_d.FirstOrDefault(wd => wd.w_o.x > 0)?.p_n ?? "Nothing");
        EditorGUILayout.LabelField("-y", nodeManager.SelectedNode.NodeSaveData.w_d.FirstOrDefault(wd => wd.w_o.y < 0)?.p_n ?? "Nothing");
        EditorGUILayout.LabelField("+y", nodeManager.SelectedNode.NodeSaveData.w_d.FirstOrDefault(wd => wd.w_o.y > 0)?.p_n ?? "Nothing");

        
        EditorGUI.indentLevel--;
        
        // Apply modifications when inspector is changed
        if (GUI.changed)
        {
            EditorUtility.SetDirty(nodeManager);
        }
    }
}