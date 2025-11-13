#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class FlagDataSetup : EditorWindow
{
    [MenuItem("Game/Setup Flag Data")]
    public static void ShowWindow()
    {
        GetWindow<FlagDataSetup>("Flag Data Setup");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Flag Data Setup", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        if (GUILayout.Button("Create Default Flag Data Asset"))
        {
            CreateFlagDataAsset();
        }
        
        GUILayout.Space(10);
        GUILayout.Label("Instructions:", EditorStyles.boldLabel);
        GUILayout.Label("1. Download flag images from Flagpedia.net or FlagLane.com");
        GUILayout.Label("2. Save flags to Assets/Sprites/Flags/");
        GUILayout.Label("3. Name files as: unitedstates.png, unitedkingdom.png, etc.");
        GUILayout.Label("4. Import as Sprites in Unity");
        GUILayout.Label("5. Assign sprites to FlagData asset");
    }
    
    private void CreateFlagDataAsset()
    {
        // Check if FlagData already exists
        FlagData existingData = Resources.Load<FlagData>("FlagData");
        
        if (existingData != null)
        {
            if (EditorUtility.DisplayDialog("Flag Data Exists", 
                "FlagData asset already exists. Do you want to overwrite it?", 
                "Yes", "No"))
            {
                existingData.InitializeDefaultFlags();
                EditorUtility.SetDirty(existingData);
                AssetDatabase.SaveAssets();
                Debug.Log("FlagData updated with default flags.");
            }
        }
        else
        {
            // Create new FlagData asset
            FlagData newData = ScriptableObject.CreateInstance<FlagData>();
            newData.InitializeDefaultFlags();
            
            // Ensure Resources folder exists
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            
            string path = "Assets/Resources/FlagData.asset";
            AssetDatabase.CreateAsset(newData, path);
            AssetDatabase.SaveAssets();
            
            Debug.Log($"FlagData asset created at {path}");
            EditorUtility.DisplayDialog("Success", "FlagData asset created successfully!", "OK");
        }
    }
}
#endif

