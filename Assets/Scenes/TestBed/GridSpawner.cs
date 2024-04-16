using System.Text;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    /// <summary>
    /// 그리드
    /// </summary>
    [SerializeField] private GameObject gridTile;
    [SerializeField] private Vector3 gridPosition;
    [SerializeField] private Vector2 gridSize;
    private GameObject grid = null;

    /// <summary>
    /// 타일
    /// </summary>
    [SerializeField] private float tileRatio;

    /// <summary>
    /// const string 변수들
    /// </summary>
    private const string GRID_NAME = "Grid";
    private const string TILE_NAME = "Tile";

    public void SpawnGrid()
    {
        //그리드, 타일들의 그룹(부모)
        CreateGridGameObject();

        //그리드 내 타일 생성
        CreateTileGameObject();
    }

    private void CreateGridGameObject()
    {
        //기존 오브젝트 삭제
        if (grid = GameObject.Find(GRID_NAME))
        {
            DestroyImmediate(grid);
        }

        //생성
        grid = new GameObject(GRID_NAME);

        //포지션
        grid.transform.position = Vector3.zero;
    }

    private void CreateTileGameObject()
    {
        //그리드를 대칭 배치 하기 위한 중점
        Vector3 center = GetCenterPosition();

        //타일 이름 설정 최적화를 위해
        StringBuilder stringBuilder = new StringBuilder();

        for (int column = 0; column < gridSize.x; column++)
        {
            for (int row = 0; row < gridSize.y; row++)
            {
                //생성
                GameObject tile = Instantiate(gridTile, grid.transform);

                //명명
                stringBuilder.Clear();
                stringBuilder.Append(TILE_NAME);
                stringBuilder.AppendFormat(" {0}, {1}", column, row);
                tile.name = stringBuilder.ToString();

                //사이즈
                Vector3 tileSize = new Vector3(tileRatio, 1, tileRatio);
                tile.transform.localScale = tileSize;

                //포지션
                Vector3 tilePosition = new Vector3(
                    center.x + (column * tileSize.x),
                    0,
                    center.z + (row * tileSize.z));

                tile.transform.position = tilePosition;
            }
        }
    }

    private Vector3 GetCenterPosition()
    {
        //그리드 사이즈의 중심 계산
        Vector3 center = new Vector3(gridSize.x, 0, gridSize.y) * tileRatio;
        center *= -0.5f;

        //타일 사이즈의 중심 계산
        Vector3 offset = Vector3.one * tileRatio * 0.5f;
        offset.y = 0;

        //사용자 설정 값 + 그리드 + 타일
        return gridPosition + center + offset;
    }
}
