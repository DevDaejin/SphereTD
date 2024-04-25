using System.Collections.Generic;
using UnityEngine;

//A* 활용
public class PathFinder : MonoBehaviour
{
    /*
     * normal은 상/하/좌/우 이동 heavy는 대각 이동
     * 10과 14인 이유는 피타고라스의 정리에서 삼각형의 빗변 구하는 공식 때문이다.
     * 상하좌우 이동을 1로 했을 때 대각은 루트2가 되므로 1.414..
     * 위 내용을 편의상 10, 14로 표현
     */
    private readonly int normal = 10;
    private readonly int heavy = 14;

    private Tile current = null;
    private List<Tile> path = new List<Tile>();

    /*
     * 1. 현재 Tile(Node)을 close(탐색 안할 노드)에 추가
     * 2. 현재 Tile의 이웃 Tile들을 open에 추가
     * (close에 이미 추가 된 Tile은 추가 하지 않음)
     * 3. 가장 유망한 타일을 current로 지정(FullCost가 더 낮거나, 동일하지만 HueristicCost가 낮은 경우)
     * 4. 반복...
     * 5. End 타일에 도달하면 Parent를 따라 경로 생성
     */
    public List<Tile> Find(Tile start, Tile end, Tile[,] tileArray)
    {
        HashSet<Tile> close = new HashSet<Tile>();
        List<Tile> open = new List<Tile>();
        open.Add(start);

        current = null;

        while (open.Count > 0)
        {
            current = open[0];
            for (int i = 1; i < open.Count; i++) 
            {
                if ((open[i].FullCost < current.FullCost) || 
                    (open[i].FullCost == current.FullCost && open[i].HueristicCost < current.HueristicCost))
                {
                    current = open[i];
                }
            }

            open.Remove(current);
            close.Add(current);

            if(current == end)
            {
                Retrace(start, end);
                return path;
            }

            foreach (Tile neighbour in current.GetNeighbours(tileArray))
            {
                if (close.Contains(neighbour)) continue;

                int newCostToNeighbour = current.GraphCost + GetDistance(current, neighbour);
                if(newCostToNeighbour < neighbour.GraphCost || !open.Contains(neighbour))
                {
                    neighbour.GraphCost = newCostToNeighbour;
                    neighbour.HueristicCost = GetDistance(neighbour, end);
                    neighbour.parnet = current;

                    if (!open.Contains(neighbour)) open.Add(neighbour);
                }
            }
        }
        Debug.LogError("Unable to go to End");
        return null;
    }

    private void Retrace(Tile start, Tile end)
    {
        path.Clear();
        current = end;

        while(current != start)
        {
            path.Add(current);
            current = current.parnet;
        }

        path.Add(start);
        path.Reverse();
    }

    int GetDistance(Tile A, Tile B)
    {
        int distX = Mathf.Abs(A.GridPosition.x - B.GridPosition.x);
        int distY = Mathf.Abs(A.GridPosition.y - B.GridPosition.y);

        //대각선으로 이동 후 남은 거리 만큼 수직/수평 이동
        if(distX > distY)
        {
            return (heavy * distY) + (normal * (distX - distY));
        }

        return (heavy * distX) + (normal * (distY - distX));
    }
}
