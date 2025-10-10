using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private bool isAddressPainting = false;
    private bool isRoomPainting = false;
    private bool isFloorTypePainting = false;
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
            isAddressPainting = GUILayout.Toggle(isAddressPainting, isAddressPainting ? "Disable Node Address Painting" : "Enable Node Address Painting", "Button");

            if (isAddressPainting)
            {
                EditorGUILayout.HelpBox(
                    "Left click: Paint address of current type\n" +
                    "Ctrl + Left click: Pick address type",
                    MessageType.Info
                );
            }
            
            isRoomPainting = GUILayout.Toggle(isRoomPainting, isRoomPainting ? "Disable Node Room Painting" : "Enable Node Room Painting", "Button");

            if (isRoomPainting)
            {
                EditorGUILayout.HelpBox(
                    "Left click: Paint room of current type\n" +
                    "Ctrl + Left click: Pick room type",
                    MessageType.Info
                );
            }
            
            isFloorTypePainting = GUILayout.Toggle(isFloorTypePainting, isFloorTypePainting ? "Disable Node FloorType Painting" : "Enable Node FloorType Painting", "Button");
            gridManager.ShowFloorAndCeilingVis = isFloorTypePainting;
            if (isFloorTypePainting)
            {
                EditorGUILayout.HelpBox(
                    "Left click: Paint current floor type\n" +
                    "Ctrl + Left click: Pick floor type",
                    MessageType.Info
                );
            }
            
            isWallPainting = GUILayout.Toggle(isWallPainting, isWallPainting ? "Disable Wall Painting" : "Enable Wall Painting", "Button");

            if (isWallPainting)
            {
                EditorGUILayout.HelpBox(
                    "Left click: Add wall of the current type\n" +
                    "Ctrl - Left click: Pick wall type\n" +
                    "Shift - Left click: Remove wall",
                    MessageType.Info
                );
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
            if (e.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                bool isPicking = e.control;
                bool isShifting = e.shift;
                
                if (Physics.Raycast(ray.origin, ray.direction, out var hitInfo))
                {
                    GridSquare square = hitInfo.collider.GetComponentInParent<GridSquare>();
                    if (square != null && e.button == 0)
                    {
                        var doneWork = false;

                        if (isAddressPainting)
                        {
                            if (isPicking)
                                addressManager.selectedAddressForPainting = addressManager.addresses.FindIndex(addr => addr == square.AddressPreset);
                            else
                                square.AddressPreset = addressManager.addresses[addressManager.selectedAddressForPainting];
                            doneWork = true;
                        }

                        if (isRoomPainting)
                        {
                            if (isPicking)
                                roomManager.selectedRoomForPainting = roomManager.rooms.FindIndex(room => room == square.RoomPreset);
                            else
                                square.RoomPreset = roomManager.rooms[roomManager.selectedRoomForPainting];
                            doneWork = true;
                        }

                        if (isFloorTypePainting)
                        {
                            if (isPicking)
                            {
                                nodeManager.SelectedFloorTileType = square.NodeSaveData.f_t;
                                nodeManager.ExtraFloorHeight = square.NodeSaveData.f_h;
                            }
                            else
                            {
                                square.NodeSaveData.f_t = nodeManager.SelectedFloorTileType;
                                square.NodeSaveData.f_h = nodeManager.ExtraFloorHeight;
                            }
                            doneWork = true;
                        }

                        if (doneWork)
                        {
                            square.UpdateVisuals();
                        }
                        else
                        {
                            nodeManager.SelectedNode = square;
                        }
                        e.Use();
                    }
                    
                    GridWall wall = hitInfo.collider.GetComponentInParent<GridWall>();
                    if (wall != null)
                    {
                        if (isWallPainting)
                        {
                            if (e.button == 0)
                            {
                                if (isPicking && wall.wallType != "")
                                {
                                    wallManager.selectedDoorWindowPreset = int.Parse(wall.wallType);
                                }
                                else if (isShifting)
                                {
                                    wall.ChangeWallType(false, "");
                                }
                                else
                                {
                                    wall.ChangeWallType(true, wallManager.selectedDoorWindowPreset.ToString());
                                }
                            }
                            wall.UpdateVisuals();
                            e.Use();
                        }
                    }
                }
            }
        }

        // This ensures the scene view updates while painting
        if (isAddressPainting || isWallPainting || isFloorTypePainting || isWallPainting)
        {
            SceneView.RepaintAll();
        }
    }
}