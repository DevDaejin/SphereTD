using UnityEngine;

[CreateAssetMenu(
    fileName ="Grid Config", 
    menuName = "Config/Grid Config", 
    order = 1)]
public class GridConfig : ScriptableObject
{
    [Header("Grid")]
    public Vector3 GridPosition;
    public Vector2Int GridSize;
    public float HeightOffset;

    [Header("Tile")]
    public GameObject GridTilePrefab;
    public float TileRatio;

    [Header("Path")]
    public GameObject TurnPointPrefab;
    public Vector2Int[] TurnPointPositionArray;
    public Material PathMaterial;
}
