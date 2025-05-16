using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WallManager))]
public class WallManagerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        WallManager wallManager = (WallManager)target;
        
        // Draw the default inspector
        DrawDefaultInspector();
        
        // Show the current preset name
        if (WallManager.DoorWindowPresets.TryGetValue(wallManager.selectedDoorWindowPreset.ToString(), out string presetName))
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Current Preset:", presetName);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
