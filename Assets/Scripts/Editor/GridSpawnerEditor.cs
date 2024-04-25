using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridManager gridSpawner = (GridManager)target;

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
