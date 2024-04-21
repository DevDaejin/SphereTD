using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridSpawner))]
public class GridSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridSpawner gridSpawner = (GridSpawner)target;

        if (gridSpawner == null )
        {
            Debug.Log("GridSpawner is null");
            return;
        }

        if(GUILayout.Button("Spawn grid"))
        {
            gridSpawner.SpawnGrid();
        }

        if(GUILayout.Button("Spawn path"))
        {
            gridSpawner.SpawnPath();
        }
    }
}
