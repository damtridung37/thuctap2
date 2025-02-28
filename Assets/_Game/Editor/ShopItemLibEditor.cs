using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
namespace D
{

    [CustomEditor(typeof(ShopItemLib))]
    public class ShopItemLibEditor : Editor
    {
        private SerializedProperty shopItems;
        private Type[] itemTypes;
        private string[] itemTypeNames;
        private int selectedTypeIndex = 0;

        private void OnEnable()
        {
            shopItems = serializedObject.FindProperty("shopItems");

            // Get all types that inherit from ShopItem
            itemTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ShopItem)))
                .ToArray();

            itemTypeNames = itemTypes.Select(type => type.Name).ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Show the list of items
            for (int i = 0; i < shopItems.arraySize; i++)
            {
                SerializedProperty element = shopItems.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(element, new GUIContent($"Item {i}"), true);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    shopItems.DeleteArrayElementAtIndex(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            // Dropdown to add new items
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add New Item", EditorStyles.boldLabel);
            selectedTypeIndex = EditorGUILayout.Popup("Item Type", selectedTypeIndex, itemTypeNames);
            if (GUILayout.Button("Add Item"))
            {
                // Create an instance of the selected type
                ShopItem newItem = (ShopItem)Activator.CreateInstance(itemTypes[selectedTypeIndex]);

                // Expand the list and assign the new instance
                shopItems.arraySize++;
                shopItems.GetArrayElementAtIndex(shopItems.arraySize - 1).managedReferenceValue = newItem;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

}
