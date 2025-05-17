using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Address))]
public class AddressDrawer : PropertyDrawer
{
    // Used to store the expanded state
    private bool isExpanded = true;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw foldout arrow
        position.height = EditorGUIUtility.singleLineHeight;
        isExpanded = EditorGUI.Foldout(position, isExpanded, label);

        if (isExpanded && property.boxedValue != null)
        {
            // Indent child fields
            EditorGUI.indentLevel++;

            // Calculate rects for the fields
            Rect addressRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width, EditorGUIUtility.singleLineHeight);
            
            Rect colorRect = new Rect(position.x, addressRect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                position.width, EditorGUIUtility.singleLineHeight);

            // Get the addressPreset property
            SerializedProperty addressPresetProp = property.FindPropertyRelative("addressPreset");
            SerializedProperty colorProp = property.FindPropertyRelative("color");

            // Draw popup and text field
            string currentValue = addressPresetProp.stringValue;
            
            // Create a list with an additional "Custom" option
            string[] options = new string[AddressPresets.AddressPreset.Length + 1];
            options[0] = "Custom";
            AddressPresets.AddressPreset.CopyTo(options, 1);

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
                new Rect(addressRect.x, addressRect.y, addressRect.width - 100, addressRect.height),
                "Address Type", currentIndex, options);
            
            // If a preset was selected, update the value
            if (EditorGUI.EndChangeCheck() && newIndex != 0)
            {
                addressPresetProp.stringValue = options[newIndex];
            }

            // Always show the text field for custom input
            string newValue = EditorGUI.TextField(
                new Rect(addressRect.x + addressRect.width - 95, addressRect.y, 95, addressRect.height),
                addressPresetProp.stringValue);
            
            if (newValue != addressPresetProp.stringValue)
            {
                addressPresetProp.stringValue = newValue;
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
            
        return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2;
    }
}