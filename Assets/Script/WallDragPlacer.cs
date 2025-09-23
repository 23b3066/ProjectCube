using System.Collections.Generic;
using UnityEngine;

public class WallDragPlacer : MonoBehaviour
{
    public static WallDragPlacer Instance;

    private Dictionary<Vector2Int, BuildingWall> wallGrid = new Dictionary<Vector2Int, BuildingWall>();

    private BuildingWall startWall = null;
    private BuildingWall currentWall = null;

    [Header("設置する壁プレハブ（インスペクターで設定）")]
    public GameObject[] wallPrefabs;  // 👈 これを追加！

    private int selectedWallIndex = 0;
    
    //パイメニューが開いているかどうか
    public bool IsPlacementEnabled { get; set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Register(BuildingWall wall)
    {
        if (!wallGrid.ContainsKey(wall.gridPos))
        {
            wallGrid.Add(wall.gridPos, wall);
            Debug.Log($"Registered wall: {wall.name}, gridPos: {wall.gridPos}, worldPos: {wall.transform.position}, IsRow: {wall.IsRow}");
        }
        else
        {
            Debug.LogWarning($"Duplicate gridPos: {wall.gridPos} already exists! wall: {wall.name}");
        }
    }

    public void OnWallHovered(BuildingWall wall)
    {
        currentWall = wall;

        if (startWall == null)
        {
            wall.ShowTemp();
            return;
        }

        ClearTempPreviews();

        var line = GetLineWalls(startWall, currentWall, startWall.IsRow);
        foreach (var w in line)
        {
            w.ShowTemp();
        }
    }

    public void OnWallUnhovered(BuildingWall wall)
    {
        if (startWall == null)
        {
            wall.HideTemp();
        }
    }

    public void OnWallSelected(BuildingWall wall)
    {
        if (startWall == null)
        {
            startWall = wall;
            currentWall = wall;
            ClearTempPreviews();
            wall.ShowTemp();
            return;
        }

        // 横向き・縦向きが異なる場合は無視
        if (wall != startWall && wall.IsRow != startWall.IsRow)
        {
            Debug.Log("StartWallと同じ方向の壁しか選べません");
            return;
        }

        currentWall = wall;

        var line = GetLineWalls(startWall, currentWall, startWall.IsRow);
        foreach (var w in line)
        {
            if (w.IsTempPlaced)
            {
                w.Confirm(wallPrefabs[selectedWallIndex]);
            }
        }
        startWall = null;
        currentWall = null;
    }

    private void ClearTempPreviews()
    {
        foreach (var wall in wallGrid.Values)
        {
            wall.HideTemp();
        }
    }

    private List<BuildingWall> GetLineWalls(BuildingWall from, BuildingWall to, bool? isRowFilter = null)
    {
        List<BuildingWall> result = new List<BuildingWall>();

        Vector2Int diff = to.gridPos - from.gridPos;

        // 斜めは無効
        if (diff.x != 0 && diff.y != 0)
            return result;

        int stepX = diff.x == 0 ? 0 : (diff.x > 0 ? 1 : -1);
        int stepY = diff.y == 0 ? 0 : (diff.y > 0 ? 1 : -1);

        Vector2Int pos = from.gridPos;

        while (true)
        {
            if (wallGrid.TryGetValue(pos, out var wall))
            {
                bool directionMatches = !isRowFilter.HasValue || wall.IsRow == isRowFilter.Value;
                bool axisMatches = true;

                if (isRowFilter.HasValue)
                {
                    if (isRowFilter.Value)
                        axisMatches = pos.y == from.gridPos.y;  // Z軸固定（横）
                    else
                        axisMatches = pos.x == from.gridPos.x;  // X軸固定（縦）
                }

                if (directionMatches && axisMatches)
                {
                    result.Add(wall);
                }
            }
            else
            {
                Debug.LogWarning($"No wall found at gridPos {pos}");
            }

            if (pos == to.gridPos)
                break;

            pos += new Vector2Int(stepX, stepY);
        }

        return result;
    }

    public void SetSelectedWallIndex(int index)
    {
        selectedWallIndex = index;
    }
}
