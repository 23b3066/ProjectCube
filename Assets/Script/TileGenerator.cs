using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public GameObject tilePrefab;   // タイルのプレハブ（3Dオブジェクト）
    public Vector3 tileSize = new Vector3(1, 1, 1); // タイルのサイズ (幅, 高さ, 奥行き)
    public float spacing = 0.2f;    // タイル間の間隔
    public int plusRow = 0;
    public int pluscolom = 0;
    public int angle = 90;
    

    private const int gridSize = 9; // 9×9グリッド
    private const int blockSize = 3; // 3×3ブロック

    void Start()
    {
        // このオブジェクトの位置を起点とする
        Vector3 startPosition = transform.position;
        GenerateTiles(startPosition);
    }

    void GenerateTiles(Vector3 startPosition)
    {
        // 全体のタイルリスト
        List<Vector2Int> selectedPositions = new List<Vector2Int>();
        Quaternion rot = Quaternion.Euler(0,0,angle);

        // 各3×3ブロックにタイルを配置
        for (int blockX = 0; blockX < gridSize + pluscolom; blockX += blockSize)
        {
            for (int blockZ = 0; blockZ < gridSize + plusRow; blockZ += blockSize)
            {
                // 3×3ブロック内の各行に1つずつタイルを配置
                for (int row = 0; row < blockSize; row++)
                {
                    int x = blockX + row; // 現在の行のx座標
                    int z = blockZ + Random.Range(0, blockSize); // ランダムな列のz座標

                    Vector2Int position = new Vector2Int(x, z);

                    // 選ばれた位置を登録
                    selectedPositions.Add(position);
                }
            }
        }

        // タイルを3D空間に配置
        foreach (Vector2Int position in selectedPositions)
        {
            Vector3 worldPosition = new Vector3(
                startPosition.x + position.x * (tileSize.x + spacing),
                startPosition.y, // 高さはそのまま
                startPosition.z + position.y * (tileSize.z + spacing)
            );
            
            Instantiate(tilePrefab, worldPosition, rot);
        }

        Debug.Log($"Generated {selectedPositions.Count} tiles.");
    }
}
