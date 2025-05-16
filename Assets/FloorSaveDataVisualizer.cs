using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.TextCore.Text;

#if UNITY_EDITOR
[CustomEditor(typeof(FloorSaveDataVisualizer))]
public class FloorSaveDataVisualizerEditor : Editor
{
    bool tileFold = false;

    public override void OnInspectorGUI()
    {
        FloorSaveDataVisualizer visualizer = (FloorSaveDataVisualizer)target;

        EditorGUILayout.LabelField("Floor Save Data Visualizer", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        visualizer.SetTextAsset((UnityEngine.TextAsset)EditorGUILayout.ObjectField(visualizer.textAsset, typeof(UnityEngine.TextAsset), true));
        
        visualizer.DrawTiles = EditorGUILayout.Toggle("Draw Tiles", visualizer.DrawTiles);
        visualizer.DrawNodes = EditorGUILayout.Toggle("Draw Nodes", visualizer.DrawNodes);
        visualizer.ShowNodeLabels = EditorGUILayout.Toggle("Show Node Labels", visualizer.ShowNodeLabels);
        visualizer.ShowNodeIndexes = EditorGUILayout.Toggle("Show Node Indexes", visualizer.ShowNodeIndexes);
        visualizer.ShowNodePath = EditorGUILayout.Toggle("Show Node JSONPath", visualizer.ShowNodePath);
        
        EditorGUILayout.Space();
        
        if (visualizer.FloorData != null)
        {
            // Floor basic info
            EditorGUILayout.LabelField("Floor Information", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Floor Name:", visualizer.FloorData.floorName);
            EditorGUILayout.Vector2Field("Size:", visualizer.FloorData.size);
            EditorGUILayout.IntField("Default Floor Height:", visualizer.FloorData.defaultFloorHeight);
            EditorGUILayout.IntField("Default Ceiling Height:", visualizer.FloorData.defaultCeilingHeight);
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            // Address data
            EditorGUILayout.LabelField("Addresses", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (var address in visualizer.FloorData.a_d)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Property Name:", address.p_n);
                EditorGUILayout.ColorField("Color:", address.e_c);
                EditorGUILayout.LabelField($"Variations Count: {address.vs.Count}");

                // If there is more than one variation, show a slider to select which variation to show
                if (address.vs.Count > 1)
                {
                    visualizer.SetSelectedVariation(address, EditorGUILayout.IntSlider("Show Variation", visualizer.SelectedVariation[address], 0,
                        address.vs.Count - 1));
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            // Tile data
            tileFold = EditorGUILayout.BeginFoldoutHeaderGroup(tileFold, "Tiles", EditorStyles.boldLabel);
            if (tileFold)
            {
                EditorGUI.indentLevel++;
                foreach (var tile in visualizer.FloorData.t_d)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.Vector2IntField("Floor Coordinates:", tile.f_c);
                    EditorGUILayout.Toggle("Is Empty:", tile.i_e);
                    EditorGUILayout.Toggle("Main Entrance:", tile.m_e);
                    EditorGUILayout.Toggle("Special Tile:", tile.s_t);
                    EditorGUILayout.IntField("Special Rotation:", tile.s_r);
                    EditorGUILayout.Toggle("Emergency Light:", tile.e_l);
                    EditorGUILayout.IntField("Emergency Rotation:", tile.e_r);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
                EditorGUI.indentLevel--;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No FloorSaveData assigned!", MessageType.Warning);
        }

        if (EditorGUILayout.LinkButton("Update"))
        {
            visualizer.ForceUpdate();
        }
    }
}

public class FloorSaveDataVisualizer : MonoBehaviour
{
    [SerializeField]
    public UnityEngine.TextAsset textAsset;

    public bool DrawTiles = true;
    public bool DrawNodes = true;
    public bool ShowNodeLabels = false;
    public bool ShowNodeIndexes = false;
    public bool ShowNodePath = false;

    public Dictionary<AddressSaveData, int> SelectedVariation = new Dictionary<AddressSaveData, int>();
    
    [NonSerialized]
    public FloorSaveData FloorData;

    public void SetTextAsset(UnityEngine.TextAsset textAsset)
    {
        if (this.textAsset != textAsset)
        {
            this.textAsset = textAsset;
            ForceUpdate();
        }
    }

    public void SetSelectedVariation(AddressSaveData address, int variation)
    {
        SelectedVariation[address] = variation;
        SceneView.RepaintAll();
    }
    
    public void ForceUpdate()
    {
        FloorData = JsonUtility.FromJson<FloorSaveData>(textAsset.text);
        SelectedVariation.Clear();
        foreach (var address in FloorData.a_d)
        {
            SelectedVariation[address] = 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (FloorData == null) return;
        
        // Draw tiles
        if (DrawTiles)
        {
            foreach (var tile in FloorData.t_d)
            {
                Vector3 tilePos = transform.position +
                                  new Vector3(
                                      tile.f_c.x * 3 + 1f,
                                      -0.05f,
                                      tile.f_c.y * 3 + 1f
                                  );

                // Different colors for different tile types
                if (tile.m_e)
                    Gizmos.color = Color.green;
                else if (tile.i_e)
                    Gizmos.color = Color.red;
                else if (tile.e_l)
                    Gizmos.color = Color.magenta;
                else if (tile.s_t)
                    Gizmos.color = Color.yellow;
                else
                    Gizmos.color = Color.gray;

                Gizmos.DrawWireCube(tilePos, new Vector3(2.9f, -.1f, 2.9f));
            }
        }


        if (DrawNodes)
        {
            // Draw address areas
            int addressIndex = 0;
            foreach (AddressSaveData address in FloorData.a_d)
            {
                Gizmos.color = address.e_c;

                if (address.vs.Count > 0)
                {
                    AddressLayoutVariation addressVariation = address.vs[SelectedVariation[address]];

                    int roomIndex = 0;
                    foreach (RoomSaveData room in addressVariation.r_d)
                    {
                        int nodeIndex = 0;
                        foreach (NodeSaveData node in room.n_d)
                        {
                            string label = "";
                            if(ShowNodeLabels)
                                label += room.l;
                            
                            if(ShowNodeIndexes)
                                label += $" ({nodeIndex}: {node.f_c.x},{node.f_c.y})";

                            if (ShowNodePath)
                                label += $" a_d/{addressIndex}/vs/{SelectedVariation[address]}/r_d/{roomIndex}/n_d/{nodeIndex}";

                            CreateLabel(new Vector3(node.f_c.x, 0, node.f_c.y), label, address.e_c);

                            foreach (var wall in node.w_d)
                            {
                                if (wall.w_o.x > 0) DrawWallAndDoor(new Vector3(node.f_c.x + 0.45f, 0, node.f_c.y), new Vector3(0.05f, 0, 1), wall.p_n);
                                if(wall.w_o.x < 0) DrawWallAndDoor(new Vector3(node.f_c.x - 0.45f, 0, node.f_c.y), new Vector3(0.05f, 0, 1), wall.p_n);
                                if(wall.w_o.y > 0) DrawWallAndDoor(new Vector3(node.f_c.x, 0, node.f_c.y + 0.45f), new Vector3(1, 0, 0.05f), wall.p_n);
                                if(wall.w_o.y < 0) DrawWallAndDoor(new Vector3(node.f_c.x, 0, node.f_c.y - 0.45f), new Vector3(1, 0, 0.05f), wall.p_n);
                            }
                            
                            nodeIndex++;
                        }
                        
                        roomIndex++;
                    }
                }

                addressIndex++;
            }
        }
        
    }

    private void CreateLabel(Vector3 position, string label, Color color)
    {
        UnityEditor.Handles.Label(position, label, new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = color }
        });
    }

    private void DrawWallAndDoor(Vector3 position, Vector3 direction, string doorLabel)
    {
        Gizmos.DrawWireCube(position, direction);
        if (doorLabel != "0")
        {
            Gizmos.DrawWireCube(position + new Vector3(0, 0.25f, 0), (direction * 0.2f) + new Vector3(0, 0.5f, 0));
            CreateLabel(position, WallManager.DoorWindowPresets[doorLabel], Color.white);
        }
    }
}
#endif