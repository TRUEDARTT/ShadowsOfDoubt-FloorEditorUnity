using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor
{
    int lastRoomsArraySize = 0;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        SerializedProperty roomTypesProperty = serializedObject.FindProperty("rooms");
        EditorGUILayout.PropertyField(roomTypesProperty, true);

        // If an element was added to the array
        if (EditorGUI.EndChangeCheck() && roomTypesProperty.arraySize > 0)
        {
            // Get the last element (newly added)
            SerializedProperty lastElement = roomTypesProperty.GetArrayElementAtIndex(roomTypesProperty.arraySize - 1);
            SerializedProperty colorProperty = lastElement.FindPropertyRelative("color");
            
            // Update the grid
            foreach (var square in serializedObject.targetObject.GetComponentsInChildren<GridSquare>())
            {
                square.UpdateVisuals();
            }
        }
        lastRoomsArraySize = roomTypesProperty.arraySize;

        // Handle the selected index
        SerializedProperty selectedIndexProperty = serializedObject.FindProperty("selectedRoomForPainting");
        
        // Calculate the max index (array size - 1, or 0 if empty)
        int maxIndex = Mathf.Max(0, roomTypesProperty.arraySize - 1);
        
        // Create a temporary label to show both the index and the room name
        string labelText = "Current Room";
        if (selectedIndexProperty.intValue >= 0 && selectedIndexProperty.intValue < roomTypesProperty.arraySize)
        {
            var roomProperty = roomTypesProperty.GetArrayElementAtIndex(selectedIndexProperty.intValue);
            var roomPresetProperty = roomProperty.FindPropertyRelative("roomPreset");
            if (roomPresetProperty != null && roomPresetProperty.stringValue != "")
            {
                labelText += $" ({roomPresetProperty.stringValue})";
            }
        }

        // Clamp the value to valid range
        selectedIndexProperty.intValue = Mathf.Clamp(selectedIndexProperty.intValue, 0, maxIndex);

        // Draw the slider
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(labelText);
        selectedIndexProperty.intValue = EditorGUILayout.IntSlider(selectedIndexProperty.intValue, 0, maxIndex);
        EditorGUILayout.EndHorizontal();

        // Preview the selected room color if valid
        if (selectedIndexProperty.intValue >= 0 && selectedIndexProperty.intValue < roomTypesProperty.arraySize)
        {
            var selectedRoom = roomTypesProperty.GetArrayElementAtIndex(selectedIndexProperty.intValue);
            var colorProperty = selectedRoom.FindPropertyRelative("color");
            if (colorProperty != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Selected Color");
                EditorGUILayout.ColorField(GUIContent.none, colorProperty.colorValue, false, false, false);
                EditorGUILayout.EndHorizontal();
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}