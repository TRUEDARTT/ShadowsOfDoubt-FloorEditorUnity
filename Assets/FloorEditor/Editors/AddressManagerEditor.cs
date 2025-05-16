using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AddressManager))]
public class AddressManagerEditor : Editor
{
    int lastAddressesArraySize = 0;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        SerializedProperty addressTypesProperty = serializedObject.FindProperty("addresses");
        EditorGUILayout.PropertyField(addressTypesProperty, true);

        
        // If an element was added to the array
        if (EditorGUI.EndChangeCheck() && addressTypesProperty.arraySize > 0)
        {
            // Get the last element (newly added)
            SerializedProperty lastElement = addressTypesProperty.GetArrayElementAtIndex(addressTypesProperty.arraySize - 1);
            SerializedProperty colorProperty = lastElement.FindPropertyRelative("color");
            
            // Set random color
            if (colorProperty != null && lastAddressesArraySize < addressTypesProperty.arraySize)
            {
                colorProperty.colorValue = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.8f, 1f);
            }
            
            // Update the grid
            foreach (var square in serializedObject.targetObject.GetComponentsInChildren<GridSquare>())
            {
                square.UpdateVisuals();
            }
            
        }
        lastAddressesArraySize = addressTypesProperty.arraySize;

        // Handle the selected index
        SerializedProperty selectedIndexProperty = serializedObject.FindProperty("selectedAddressForPainting");
        
        // Calculate the max index (array size - 1, or 0 if empty)
        int maxIndex = Mathf.Max(0, addressTypesProperty.arraySize - 1);
        
        // Create a temporary label to show both the index and the address name
        string labelText = "Current Address";
        if (selectedIndexProperty.intValue >= 0 && selectedIndexProperty.intValue < addressTypesProperty.arraySize)
        {
            var addressProperty = addressTypesProperty.GetArrayElementAtIndex(selectedIndexProperty.intValue);
            var addressPresetProperty = addressProperty.FindPropertyRelative("addressPreset");
            if (addressPresetProperty != null && addressPresetProperty.stringValue != "")
            {
                labelText += $" ({addressPresetProperty.stringValue})";
            }
        }

        // Clamp the value to valid range
        selectedIndexProperty.intValue = Mathf.Clamp(selectedIndexProperty.intValue, 0, maxIndex);

        // Draw the slider
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(labelText);
        selectedIndexProperty.intValue = EditorGUILayout.IntSlider(selectedIndexProperty.intValue, 0, maxIndex);
        EditorGUILayout.EndHorizontal();

        // Preview the selected address color if valid
        if (selectedIndexProperty.intValue >= 0 && selectedIndexProperty.intValue < addressTypesProperty.arraySize)
        {
            var selectedAddress = addressTypesProperty.GetArrayElementAtIndex(selectedIndexProperty.intValue);
            var colorProperty = selectedAddress.FindPropertyRelative("color");
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