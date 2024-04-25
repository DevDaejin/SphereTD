using System.Collections.Generic;
using System.Text;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GridManager : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private GridConfig gridConfig;
    private GameObject grid = null;

    [Header("Path")]
    private TurnningPoint[] turnPointArray;
    private PathFinder pathFinder;
    private GameObject path = null;

    [Header("Nav")]
    [SerializeField] Transform navMesh;
    NavMeshSurface navMeshSurface = null;

    //title
    private Tile[,] tileArray;
   
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
        if (gridConfig.HeightOffset < GRID_HEIGHT_OFFSET_MINIMUN)
            gridConfig.HeightOffset = GRID_HEIGHT_OFFSET_MINIMUN;

        //그리드, 타일들의 그룹(부모)
        InitGrid();

        //그리드 내 타일 생성
        CreateTileGameObject();

        //그리드 사이즈에 맞춰 Nav mesh 스케일 조정
        navMesh.position = gridConfig.GridPosition;
        navMesh.localScale = new Vector3(gridConfig.GridSize.x * gridConfig.TileRatio,
                                          gridConfig.GridSize.y * gridConfig.TileRatio, 1);

        //Nav bake
        if (navMeshSurface == null) navMeshSurface = navMesh.GetComponentInParent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();
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
        GenerateTurnningPoint();
    }

    private void InitGrid()
    {
        //기존 오브젝트 삭제
        grid = GameObject.Find(GRID_NAME);
        if (grid)
        {
            DestroyImmediate(grid);
        }

        //생성
        grid = new GameObject(GRID_NAME);
        //포지션
        grid.transform.SetParent(transform);
        grid.transform.position = Vector3.up * gridConfig.HeightOffset;
    }

    private void CreateTileGameObject()
    {
        tileArray = new Tile[gridConfig.GridSize.x, gridConfig.GridSize.y];

        //그리드를 대칭 배치 하기 위한 중점
        Vector3 center = GetCenterPosition();

        //타일 이름 설정 최적화를 위해
        StringBuilder stringBuilder = new StringBuilder();

        for (int x = 0; x < gridConfig.GridSize.x; x++)
        {
            for (int y = 0; y < gridConfig.GridSize.y; y++)
            {
                //생성
                GameObject tile = Instantiate(gridConfig.GridTilePrefab, grid.transform);

                //명명
                stringBuilder.Clear();
                stringBuilder.Append(TILE_NAME).AppendFormat(" {0}, {1}", x, y);
                tile.name = stringBuilder.ToString();

                //사이즈
                Vector3 tileSize = new Vector3(gridConfig.TileRatio, 1, gridConfig.TileRatio);
                tile.transform.localScale = tileSize;

                //포지션
                tile.transform.localPosition = new Vector3(
                    center.x + (x * tileSize.x),
                    0,
                    center.z + ((gridConfig.GridSize.y - 1 - y) * tileSize.z));
                //그리드 원점(0, 0)을 좌상단으로
                tileArray[x, y] = new Tile(tile.gameObject, new Vector2Int(x, y));
            }
        }
    }

    private Vector3 GetCenterPosition()
    {
        //그리드 사이즈의 중심 계산
        Vector3 center = new Vector3(gridConfig.GridSize.x, 0, gridConfig.GridSize.y) * gridConfig.TileRatio;
        center *= -0.5f;

        //타일 사이즈의 중심 계산
        Vector3 offset = Vector3.one * gridConfig.TileRatio * 0.5f;
        offset.y = 0;

        //사용자 설정 값 + 그리드 + 타일
        return gridConfig.GridPosition + center + offset;
    }

    private void InitPath()
    {
        //기존 오브젝트 삭제
        path = GameObject.Find(PATH_NAME);
        if (path)
        {
            DestroyImmediate(path);
        }

        //생성
        path = new GameObject(PATH_NAME);

        //포지션
        path.transform.SetParent(transform);
        path.transform.position = grid.transform.position + (Vector3.down * PATH_HEIGHT_OFFSET);

        //캐싱 or 생성
        if (pathFinder == null)
        {
            pathFinder = GetComponent<PathFinder>() ?? gameObject.AddComponent<PathFinder>();
        }

        //적 이동 패쓰 메테리얼
        if (gridConfig.PathMaterial == null)
        {
            gridConfig.PathMaterial = Resources.Load<Material>(PATH_MATERIAL);

            if (gridConfig.PathMaterial == null) Debug.LogError("Tile material is null");
        }
    }

    private List<Tile> GetPath()
    {//사용자가 입력한 turnning point 간의 최소 거리 수집 후 전달
        List<Tile> pathTileList = new List<Tile>();
        if (gridConfig.TurnPointPositionArray.Length < 2)
        {
            Debug.LogError("Check turnning point array");
        }

        for (int i = 0; i < gridConfig.TurnPointPositionArray.Length - 1; i++)
        {
            pathTileList.AddRange(pathFinder.Find(
                GetTile(gridConfig.TurnPointPositionArray[i]), 
                GetTile(gridConfig.TurnPointPositionArray[i + 1]), tileArray));
        }

        return pathTileList;
    }

    private Tile GetTile(Vector2Int position)
    {//특정 tile 가져오기
        if (position.x >= 0 && position.x < gridConfig.GridSize.x &&
           position.y >= 0 && position.y < gridConfig.GridSize.y)
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
                mrArray[mrIndex].material = gridConfig.PathMaterial;
            }
        }
    }

    private void GenerateTurnningPoint()
    {
        int next = 0;
        for (int index = 0; index < gridConfig.TurnPointPositionArray.Length; index++)
        {
            TurnningPoint turnningPoint = 
                Instantiate(gridConfig.TurnPointPrefab, path.transform).GetComponent<TurnningPoint>();

            turnningPoint.transform.position = 
                GetTile(gridConfig.TurnPointPositionArray[index]).gameObject.transform.position + (Vector3.up * INDICATOR_HEIGHT_OFFSET);


            next = index + 1;
            if (next >= gridConfig.TurnPointPositionArray.Length)
                next = gridConfig.TurnPointPositionArray.Length - 1;

            switch (index)
            {
                case 0:
                    turnningPoint.Init(
                        INDICATOR_1, 
                        GetTile(gridConfig.TurnPointPositionArray[next]).gameObject.transform.position);
                    break;
                case 1:
                    turnningPoint.Init(
                        INDICATOR_2, 
                        GetTile(gridConfig.TurnPointPositionArray[next]).gameObject.transform.position);
                    break;
                case 2:
                    turnningPoint.Init(
                        INDICATOR_3, 
                        GetTile(gridConfig.TurnPointPositionArray[next]).gameObject.transform.position);
                    break;
                case 3:
                    turnningPoint.Init(
                        INDICATOR_4, 
                        GetTile(gridConfig.TurnPointPositionArray[next]).gameObject.transform.position);
                    break;
                case 4:
                    turnningPoint.Init(
                        INDICATOR_5, 
                        GetTile(gridConfig.TurnPointPositionArray[next]).gameObject.transform.position);
                    break;
                case 5:
                    turnningPoint.Init(
                        INDICATOR_6, 
                        GetTile(gridConfig.TurnPointPositionArray[next]).gameObject.transform.position);
                    break;
                case 6:
                    turnningPoint.Init(
                        INDICATOR_7, 
                        GetTile(gridConfig.TurnPointPositionArray[next]).gameObject.transform.position);
                    break;
            }
        }
    }
}