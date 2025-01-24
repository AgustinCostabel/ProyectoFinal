using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class AS_TreeTerrain : MonoBehaviour {
    public Terrain terrain;
    private TreeInstance[] _originalTrees;

    void Start() {
        terrain = GetComponent<Terrain>();
        // backup original terrain trees
        _originalTrees = terrain.terrainData.treeInstances;
        for (int i = 0; i < terrain.terrainData.treeInstances.Length; i++) {
            TreeInstance treeInstance = terrain.terrainData.treeInstances[i];
        }
    }
}
