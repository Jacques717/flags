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
            int currentCount = existingData.flags != null ? existingData.flags.Count : 0;
            if (EditorUtility.DisplayDialog("Flag Data Exists", 
                $"FlagData asset already exists with {currentCount} flags.\n\nThis will update it with ALL countries from around the world (~195+ countries).\n\nDo you want to overwrite it?", 
                "Yes, Update", "Cancel"))
            {
                existingData.InitializeDefaultFlags();
                EditorUtility.SetDirty(existingData);
                AssetDatabase.SaveAssets();
                int newCount = existingData.flags != null ? existingData.flags.Count : 0;
                Debug.Log($"FlagData updated with {newCount} countries from around the world.");
                EditorUtility.DisplayDialog("Update Complete", 
                    $"FlagData has been updated!\n\nTotal countries: {newCount}\n\nThe game will now randomly select from all countries worldwide.", 
                    "OK");
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
            
            int count = newData.flags != null ? newData.flags.Count : 0;
            Debug.Log($"FlagData asset created at {path} with {count} countries");
            EditorUtility.DisplayDialog("Success", 
                $"FlagData asset created successfully!\n\nTotal countries: {count}\n\nThe game will now randomly select from all countries worldwide.", 
                "OK");
        }
    }
}
#endif

