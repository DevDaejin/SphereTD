using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public GameObject gameObject;
    public Tile parnet;
    public Vector2Int GridPosition;

    /// 코스트 관련
    public int GraphCost;   //시작점부터 현재 타일까지 경로를 따라 이동한 비용
    public int HueristicCost;   //현재 타일에서 목적지까지 이동한 예상 비용
    public int FullCost
    {//현재까지 이동하는데 걸린 비용과 예상 비용의 합산 비용
        get => GraphCost + HueristicCost;
    }

    public Tile(GameObject gameObject, Vector2Int GridPosition)
    {
        this.gameObject = gameObject;
        this.GridPosition = GridPosition;
    }

    public IEnumerable<Tile> GetNeighbours(Tile[,] tileArray)
    {
        List<Tile> neighbours = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //자기 자신
                if (x == 0 && y == 0) continue;

                Vector2Int neighboursGridPosition = 
                    new Vector2Int(GridPosition.x + x, GridPosition.y + y);

                //그리드 내에 있는지
                if(neighboursGridPosition.x >= 0 &&
                   neighboursGridPosition.x < tileArray.GetLength(0) &&
                   neighboursGridPosition.y >= 0 &&
                   neighboursGridPosition.y < tileArray.GetLength(1))
                {
                    neighbours.Add(tileArray
                        [
                            neighboursGridPosition.x, 
                            neighboursGridPosition.y
                        ]);
                }
            }
        }

        return neighbours;
    }
}
