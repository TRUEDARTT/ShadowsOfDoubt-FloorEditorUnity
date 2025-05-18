using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Room))]
public class RoomDrawer : PropertyDrawer
{
    private bool isExpanded = true;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position.height = EditorGUIUtility.singleLineHeight;
        isExpanded = EditorGUI.Foldout(position, isExpanded, label);

        if (isExpanded && property.boxedValue != null)
        {
            EditorGUI.indentLevel++;

            // Calculate rects for the fields
            Rect idRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width, EditorGUIUtility.singleLineHeight);
            
            Rect roomTypeRect = new Rect(position.x, idRect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width, EditorGUIUtility.singleLineHeight);
            
            Rect colorRect = new Rect(position.x, roomTypeRect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width, EditorGUIUtility.singleLineHeight);

            // Get the properties
            SerializedProperty idProp = property.FindPropertyRelative("id");
            SerializedProperty roomPresetProp = property.FindPropertyRelative("roomPreset");
            SerializedProperty colorProp = property.FindPropertyRelative("color");

            // Draw ID field
            EditorGUI.PropertyField(idRect, idProp);

            // Draw room preset popup and text field
            string currentValue = roomPresetProp.stringValue;
            
            // Create a list with an additional "Custom" option
            string[] options = new string[RoomPresets.RoomPreset.Length + 1];
            options[0] = "Custom";
            RoomPresets.RoomPreset.CopyTo(options, 1);

            // Find the current index
            int currentIndex = 0; // Default to "Custom"
            for (int i = 1; i < options.Length; i++)
            {
                if (options[i] == currentValue)
                {
                    currentIndex = i;
                    break;
                }
            }

            // Draw the popup
            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUI.Popup(
                new Rect(roomTypeRect.x, roomTypeRect.y, roomTypeRect.width - 200, roomTypeRect.height),
                "Room Type", currentIndex, options);
            
            // If a preset was selected, update the value
            if (EditorGUI.EndChangeCheck() && newIndex != 0)
            {
                roomPresetProp.stringValue = options[newIndex];
            }

            // Always show the text field for custom input
            string newValue = EditorGUI.TextField(
                new Rect(roomTypeRect.x + roomTypeRect.width - 195, roomTypeRect.y, 195, roomTypeRect.height),
                roomPresetProp.stringValue);
            
            if (newValue != roomPresetProp.stringValue)
            {
                roomPresetProp.stringValue = newValue;
            }

            // Draw the color field
            EditorGUI.PropertyField(colorRect, colorProp);

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!isExpanded)
            return EditorGUIUtility.singleLineHeight;
            
        return EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 3;
    }
}