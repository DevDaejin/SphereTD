using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private GameObject gridTile;
    [SerializeField] private Vector3 gridPosition;
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float heightOffset;//바닥 오브젝트와 z-fighting 방지
    private GameObject grid = null;

    [Header("Tile")]
    [SerializeField] private float tileRatio;
    private Tile[,] tileArray;

    [Header("Path")]
    [SerializeField] private Vector2Int[] turnningPointArray;
    private PathFinder pathFinder;
    private GameObject path = null;
    private Material pathMaterial;

    [SerializeField] private GameObject indicator;



    /// const string 변수들
    private const string GRID_NAME = "Grid";
    private const string TILE_NAME = "Tile";
    private const string PATH_NAME = "Path";
    private const string PATH_MATERIAL = "Materials/Path";
    private const string INDICATOR_1 = "Start";
    private const string INDICATOR_2 = "1";
    private const string INDICATOR_3 = "2";
    private const string INDICATOR_4 = "3";
    private const string INDICATOR_5 = "4";
    private const string INDICATOR_6 = "5";
    private const string INDICATOR_7 = "End";
    private const float GRID_HEIGHT_OFFSET_MINIMUN = 0.001f;
    private const float PATH_HEIGHT_OFFSET = 0.0005f;
    private const float INDICATOR_HEIGHT_OFFSET = 0.0005f;

    public void SpawnGrid()
    {
        //그리드 높이 최소값 설정
        if (heightOffset < GRID_HEIGHT_OFFSET_MINIMUN)
            heightOffset = GRID_HEIGHT_OFFSET_MINIMUN;

        //그리드, 타일들의 그룹(부모)
        InitGrid();

        //그리드 내 타일 생성
        CreateTileGameObject();
    }

    public void SpawnPath()
    {//적의 이동 경로

        //패쓰, 적의 이동 경로 타일들의 그룹(부모)
        InitPath();

        //그리드 내 패쓰 해당 타일
        List<Tile> path = GetPath();

        //패쓰 생성
        GeneratePath(path);

        //터닝 포인트 표시
        GenerateTurnningPointIndicator();
    }

    private void InitGrid()
    {
        //기존 오브젝트 삭제
        if (grid = GameObject.Find(GRID_NAME))
        {
            DestroyImmediate(grid);
        }

        //생성
        grid = new GameObject(GRID_NAME);

        //포지션
        grid.transform.position = Vector3.up * heightOffset;
    }

    private void CreateTileGameObject()
    {
        tileArray = new Tile[gridSize.x, gridSize.y];

        //그리드를 대칭 배치 하기 위한 중점
        Vector3 center = GetCenterPosition();

        //타일 이름 설정 최적화를 위해
        StringBuilder stringBuilder = new StringBuilder();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                //생성
                GameObject tile = Instantiate(gridTile, grid.transform);

                //명명
                stringBuilder.Clear();
                stringBuilder.Append(TILE_NAME).AppendFormat(" {0}, {1}", x, y);
                tile.name = stringBuilder.ToString();

                //사이즈
                Vector3 tileSize = new Vector3(tileRatio, 1, tileRatio);
                tile.transform.localScale = tileSize;

                //포지션
                tile.transform.localPosition = new Vector3(
                    center.x + (x * tileSize.x),
                    0,
                    center.z + ((gridSize.y - 1 - y) * tileSize.z));
                //그리드 원점(0, 0)을 좌상단으로
                tileArray[x, y] = new Tile(tile.gameObject, new Vector2Int(x, y));
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

    private void InitPath()
    {
        //기존 오브젝트 삭제
        if (path = GameObject.Find(PATH_NAME))
        {
            DestroyImmediate(path);
        }

        //생성
        path = new GameObject(PATH_NAME);

        //포지션
        path.transform.position = grid.transform.position + (Vector3.down * PATH_HEIGHT_OFFSET);

        //캐싱 or 생성
        if (pathFinder == null)
        {
            pathFinder = GetComponent<PathFinder>() ?? gameObject.AddComponent<PathFinder>();
        }

        //적 이동 패쓰 메테리얼
        if (pathMaterial == null)
        {
            pathMaterial = Resources.Load<Material>(PATH_MATERIAL);

            if (pathMaterial == null) Debug.LogError("Tile material is null");
        }
    }

    private List<Tile> GetPath()
    {//사용자가 입력한 turnning point 간의 최소 거리 수집 후 전달
        List<Tile> pathTileList = new List<Tile>();
        if (turnningPointArray.Length < 2)
        {
            Debug.LogError("Check turnning point array");
        }

        for (int i = 0; i < turnningPointArray.Length - 1; i++)
        {
            pathTileList.AddRange(pathFinder.Find(GetTile(turnningPointArray[i]), GetTile(turnningPointArray[i + 1]), tileArray));
        }

        return pathTileList;
    }

    private Tile GetTile(Vector2Int position)
    {//특정 tile 가져오기
        if (position.x >= 0 && position.x < gridSize.x &&
           position.y >= 0 && position.y < gridSize.y)
        {
            return tileArray[position.x, position.y];
        }

        Debug.LogError("Maybe \"Position\" is not in the grid. ");
        return null;
    }

    private void GeneratePath(List<Tile> pathTileList)
    {//매개변수 타일을 복수하여 적 이동경로로 사용
        GameObject go;
        for (int pathIndex = 0; pathIndex < pathTileList.Count; pathIndex++)
        {
            go = Instantiate(pathTileList[pathIndex].gameObject, path.transform);

            MeshRenderer[] mrArray = go.GetComponentsInChildren<MeshRenderer>();

            for (int mrIndex = 0; mrIndex < mrArray.Length; mrIndex++)
            {
                mrArray[mrIndex].material = pathMaterial;
            }
        }
    }

    private void GenerateTurnningPointIndicator()
    {//터닝 포인트 명시
        for (int index = 0; index < turnningPointArray.Length; index++)
        {
            GameObject go = Instantiate(indicator, path.transform);
            TMP_Text tmpText = go.GetComponentInChildren<TMP_Text>();

            go.transform.position = 
                GetTile(turnningPointArray[index]).gameObject.transform.position + (Vector3.up * INDICATOR_HEIGHT_OFFSET);

            switch (index)
            {
                case 0:
                    go.name = INDICATOR_1;
                    tmpText.text = INDICATOR_1;
                    break;
                case 1:
                    go.name = INDICATOR_2;
                    tmpText.text = INDICATOR_2;
                    break;
                case 2:
                    go.name = INDICATOR_3;
                    tmpText.text = INDICATOR_3;
                    break;
                case 3:
                    go.name = INDICATOR_4;
                    tmpText.text = INDICATOR_4;
                    break;
                case 4:
                    go.name = INDICATOR_5;
                    tmpText.text = INDICATOR_5;
                    break;
                case 5:
                    go.name = INDICATOR_6;
                    tmpText.text = INDICATOR_6;
                    break;
                case 6:
                    go.name = INDICATOR_7;
                    tmpText.text = INDICATOR_7;
                    break;
            }
        }
    }
}