using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(DataBuilder))]
public class DataBuilderEditor : Editor
{
    private string[] gameFloorPaths;
    private string[] modFloorPaths;
    private string[] allFloorOptions;
    private int selectedFloorIndex = -1;
    private string searchString = "";
    private Vector2 scrollPosition;
    private bool isDropdownOpen;


    private void OnEnable()
    {
        // Get paths for base game floors
        string gameFloorsPath = "Assets/GameFloorExports";
        gameFloorPaths = Directory.Exists(gameFloorsPath) 
            ? Directory.GetFiles(gameFloorsPath, "*.txt")
            : new string[0];

        // Get paths for modded floors
        string modFloorsPath = "Assets/FloorSaves";
        modFloorPaths = Directory.Exists(modFloorsPath)
            ? Directory.GetFiles(modFloorsPath, "*.json")
            : new string[0];

        // Combine both arrays with headers for the popup
        allFloorOptions = new string[gameFloorPaths.Length + modFloorPaths.Length + 2];
        
        allFloorOptions[0] = "Modded Floors:";
        for (int i = 0; i < modFloorPaths.Length; i++)
        {
            allFloorOptions[i + 1] = Path.GetFileNameWithoutExtension(modFloorPaths[i]);
        }
        
        allFloorOptions[modFloorPaths.Length + 1] = "Base Game Floors:";
        for (int i = 0; i < gameFloorPaths.Length; i++)
        {
            allFloorOptions[i + modFloorPaths.Length + 2] = Path.GetFileNameWithoutExtension(gameFloorPaths[i]);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DataBuilder dataBuilder = (DataBuilder)target;

        EditorGUI.BeginChangeCheck();
        
        // Custom searchable dropdown
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Select Floor");
        
        if (EditorGUILayout.DropdownButton(
            new GUIContent(selectedFloorIndex >= 0 ? allFloorOptions[selectedFloorIndex] : "Select a floor..."),
            FocusType.Keyboard))
        {
            isDropdownOpen = !isDropdownOpen;
        }
        EditorGUILayout.EndHorizontal();

        if (isDropdownOpen)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            searchString = EditorGUILayout.TextField("Search", searchString);
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(180));
            
            for (int i = 0; i < allFloorOptions.Length; i++)
            {
                if (string.IsNullOrEmpty(searchString) || 
                    allFloorOptions[i].ToLower().Contains(searchString.ToLower()))
                {
                    // Headers in bold
                    if (i == 0 || i == modFloorPaths.Length + 1)
                    {
                        EditorGUILayout.LabelField(allFloorOptions[i], EditorStyles.boldLabel);
                        continue;
                    }

                    if (GUILayout.Button(allFloorOptions[i], EditorStyles.label))
                    {
                        selectedFloorIndex = i;
                        isDropdownOpen = false;
                        GUI.FocusControl(null);
                    }
                }
            }
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        if (EditorGUI.EndChangeCheck() && selectedFloorIndex >= 0)
        {
            // Rest of the selection handling code remains the same
            if (selectedFloorIndex != 0 && selectedFloorIndex != modFloorPaths.Length + 1)
            {
                string path;
                if (selectedFloorIndex <= modFloorPaths.Length)
                {
                    path = modFloorPaths[selectedFloorIndex - 1];
                }
                else
                {
                    path = gameFloorPaths[selectedFloorIndex - modFloorPaths.Length - 2];
                }
                
                dataBuilder.textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            }
        }


        // Display current text asset
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Current Floor Data", dataBuilder.textAsset, typeof(TextAsset), false);
        EditorGUI.EndDisabledGroup();
        
        dataBuilder.floorName = EditorGUILayout.TextField("Floor Name", dataBuilder.floorName);

        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("Load Floor Data"))
        {
            if (dataBuilder.textAsset != null)
            {
                dataBuilder.Load();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please select a floor file to load.", "OK");
            }
        }

        if (GUILayout.Button("Save Floor Data"))
        {
            if (!string.IsNullOrEmpty(dataBuilder.floorName))
            {
                dataBuilder.Save();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please enter a floor name before saving.", "OK");
            }
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}