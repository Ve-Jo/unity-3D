using UnityEngine;

public class WallTextureAdjuster : MonoBehaviour
{
    [SerializeField]
    private float baseTextureSize = 1f; // Size in units that represents 1 tile
    
    void Start()
    {
        AdjustTexturesInChildren();
    }

    private void AdjustTexturesInChildren()
    {
        // Get all MeshRenderer components in children
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            // Get the mesh filter
            MeshFilter meshFilter = renderer.GetComponent<MeshFilter>();
            if (meshFilter == null) continue;

            Transform objTransform = renderer.transform;
            
            // Get the actual dimensions using local scale and mesh bounds
            Vector3 meshSize = meshFilter.sharedMesh.bounds.size;
            Vector3 objectScale = objTransform.lossyScale;
            
            // Calculate real world dimensions
            float realWidth = Mathf.Abs(meshSize.x * objectScale.x);
            float realHeight = Mathf.Abs(meshSize.y * objectScale.y);
            
            // Get the material
            Material material = renderer.material;
            
            // Calculate tiling values
            float tilingX = realWidth / baseTextureSize;
            float tilingY = realHeight / baseTextureSize;
            
            // Set the tiling in the material
            material.mainTextureScale = new Vector2(tilingX, tilingY);
            
            // Make sure each wall has its own material instance
            renderer.material = material;
        }
    }

    // Optional: Add a button in the inspector to recalculate
    [ContextMenu("Recalculate Textures")]
    private void RecalculateTextures()
    {
        AdjustTexturesInChildren();
    }
} 