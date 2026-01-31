using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PickItems : MonoBehaviour
{
    public string folderPath = "Assets/Models/Random Items";
    
    [HideInInspector] public List<Mesh> foundMeshes = new List<Mesh>();
    [Header("Selection")]
    public int itemIndex;

    // This runs in the editor whenever you change a value
    private void OnValidate()
    {
        #if UNITY_EDITOR
        // Only scan if the list is empty or you want to refresh
        if (foundMeshes.Count == 0) 
        {
            RefreshAssetList();
        }
        #endif

        ApplySelection();
    }

    public void ApplySelection()
    {
        if (foundMeshes == null || foundMeshes.Count == 0) return;

        // Keep index in range
        itemIndex = Mathf.Clamp(itemIndex, 0, foundMeshes.Count - 1);
        Mesh targetMesh = foundMeshes[itemIndex];

        // 1. Mesh Setup
        MeshFilter mf = GetOrAddComponent<MeshFilter>();
        MeshRenderer mr = GetOrAddComponent<MeshRenderer>();
        mf.sharedMesh = targetMesh;

        // 2. MeshCollider: Convex = true
        MeshCollider mc = GetOrAddComponent<MeshCollider>();
        mc.sharedMesh = targetMesh;
        mc.convex = true;
        // mc.Material = ;

        // 3. Rigidbody: Drag = 0.3
        Rigidbody rb = GetOrAddComponent<Rigidbody>();
        rb.drag = 0.3f;

        // 4. PickUp Script (Assuming the script name is PickUp)
        if (GetComponent<PickUp>() == null)
        {
            gameObject.AddComponent<PickUp>();
        }

        // Rename for clarity in Hierarchy
        this.name = $"Item_{targetMesh.name}";
    }

    #if UNITY_EDITOR
    [ContextMenu("Refresh Folder Assets")]
    public void RefreshAssetList()
    {
        foundMeshes.Clear();

        // Find all Model files (FBX, OBJ, etc.) in the folder
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { folderPath });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            // Load the main asset (the model)
            GameObject modelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            if (modelPrefab != null)
            {
                // Grab the mesh from the first MeshFilter found in the model
                MeshFilter meshFilter = modelPrefab.GetComponentInChildren<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    foundMeshes.Add(meshFilter.sharedMesh);
                }
            }
        }
        Debug.Log($"Found {foundMeshes.Count} meshes in {folderPath}");
    }
    #endif

    private T GetOrAddComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        if (component == null) component = gameObject.AddComponent<T>();
        return component;
    }
}