using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private bool isNodePainting = false;
    private bool isWallPainting = false;
    
    private bool showGridControls = false;
    private bool showPaintingTools = true;

    public override void OnInspectorGUI()
    {
        GridManager gridManager = (GridManager)target;
        AddressManager addressManager = gridManager.GetComponent<AddressManager>();
        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("Create/Refresh Grid"))
        {
            Undo.RecordObject(gridManager, "Create Grid");
            gridManager.CreateGrid();
            EditorSceneManager.MarkSceneDirty(gridManager.gameObject.scene);
        }
        
        showGridControls = EditorGUILayout.Foldout(showGridControls, "Grid Controls", true);
        if (showGridControls)
        {
            EditorGUI.indentLevel++;
            
            gridManager.width = EditorGUILayout.IntField("Width", gridManager.width);
            gridManager.height = EditorGUILayout.IntField("Height", gridManager.height);
            gridManager.cellSize = EditorGUILayout.FloatField("Cell Size", gridManager.cellSize);
            gridManager.squarePrefab = (GameObject)EditorGUILayout.ObjectField(
                "Square Prefab", 
                gridManager.squarePrefab, 
                typeof(GameObject), 
                false
            );
            gridManager.wallPrefab = (GameObject)EditorGUILayout.ObjectField(
                "Wall Prefab", 
                gridManager.wallPrefab, 
                typeof(GameObject), 
                false
            );

            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            if (GUILayout.Button("Clear Grid"))
            {
                Undo.RecordObject(gridManager, "Clear Grid");
                gridManager.ClearGrid();
                EditorSceneManager.MarkSceneDirty(gridManager.gameObject.scene);
            }
        }

        EditorGUILayout.Space();
        showPaintingTools = EditorGUILayout.Foldout(showPaintingTools, "Painting Tools", true);
        if (showPaintingTools && addressManager.addresses.Count > 0)
        {
            isNodePainting = GUILayout.Toggle(isNodePainting, isNodePainting ? "Disable Node Painting Mode" : "Enable Node Painting Mode", "Button");

            if (isNodePainting)
            {
                EditorGUILayout.HelpBox(
                    "Left click: Paint color and room type\n" +
                    "Shift + Left click: Paint room type only\n" +
                    "Ctrl + Left click: Paint color only\n\n" +
                    "Right click: Pick color and room type\n" +
                    "Shift + Right click: Pick room type only\n" +
                    "Ctrl + Right click: Pick color only", 
                    MessageType.Info
                );

                isWallPainting = false;
            }
            
            isWallPainting = GUILayout.Toggle(isWallPainting, isWallPainting ? "Disable Wall Mode" : "Enable Wall Mode", "Button");

            if (isWallPainting)
            {
                EditorGUILayout.HelpBox(
                    "Left click: Add wall of the current type\n" +
                    "Ctrl - Left click: Remove wall\n" +
                    "Right click: Pick wall type",
                    MessageType.Info
                );

                isNodePainting = false;
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void OnSceneGUI()
    {
        GridManager gridManager = (GridManager)target;
        AddressManager addressManager = gridManager.GetComponent<AddressManager>();
        RoomManager roomManager = gridManager.GetComponent<RoomManager>();
        NodeManager nodeManager = gridManager.GetComponent<NodeManager>();
        WallManager wallManager = gridManager.GetComponent<WallManager>();
        
        Event e = Event.current;

        if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
        {
            if (e.button == 0 || e.button == 1)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                
                if (Physics.Raycast(ray.origin, ray.direction, out var hitInfo))
                {
                    GridSquare square = hitInfo.collider.GetComponentInParent<GridSquare>();
                    if (square != null)
                    {
                        if (isNodePainting)
                        {
                            bool shouldPaintAddress = !e.shift;
                            bool shouldPaintRoom = !e.control;
                            
                            // LMB - Paint
                            if (e.button == 0)
                            {
                                if (shouldPaintAddress)
                                {
                                    square.SetAddressPreset(addressManager.addresses[addressManager.selectedAddressForPainting]);
                                }
                                if (shouldPaintRoom)
                                { 
                                    square.SetRoomType(roomManager.rooms[roomManager.selectedRoomForPainting]);
                                }
                                
                                EditorUtility.SetDirty(square);
                                EditorSceneManager.MarkSceneDirty(square.gameObject.scene);
                                
                                e.Use();
                            }
                            // RMB - Sample
                            // TODO: Don't sample null
                            else if (e.button == 1)
                            {
                                if (shouldPaintAddress)
                                {
                                    addressManager.selectedAddressForPainting = addressManager.addresses.FindIndex(addr => addr == square.AddressPreset);
                                }
                                if (shouldPaintRoom)
                                { 
                                    roomManager.selectedRoomForPainting = roomManager.rooms.FindIndex(room => room == square.RoomPreset);
                                }
                                e.Use();
                            }
                        }
                        else
                        {
                            if (e.button == 0)
                            {
                                nodeManager.SelectedNode = square;
                                e.Use();
                            }
                        }
                    }
                    
                    GridWall wall = hitInfo.collider.GetComponentInParent<GridWall>();
                    if (wall != null)
                    {
                        if (isWallPainting)
                        {
                            if (e.button == 0)
                            {
                                if (e.control)
                                {
                                    wall.ChangeWallType(false, "");
                                }
                                else
                                {
                                    wall.ChangeWallType(true, wallManager.selectedDoorWindowPreset.ToString());
                                }
                                e.Use();
                            }
                            else if (e.button == 1 && wall.wallType != "")
                            {
                                wallManager.selectedDoorWindowPreset = int.Parse(wall.wallType);
                                e.Use();
                            }
                        }
                    }
                }
            }
        }

        // This ensures the scene view updates while painting
        if (isNodePainting || isWallPainting)
        {
            SceneView.RepaintAll();
        }
    }
}