using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneratorRandom : MonoBehaviour
{
    [Header("タイル設定")]
    public GameObject tilePrefab;
    public Vector3 tileSize = new Vector3(1, 1, 1);
    public float spacing = 0.2f;
    public int plusRow = 0;
    public int plusColom = 0;
    public int angle = 90;

    [Header("タイル上下動設定")]
    public float lowY = 0f;
    public float midY = 2f;
    public float highY = 4f;
    public float moveSpeed = 2f;
    public float stayTime = 1.5f;

    private const int gridSize = 9;
    private const int blockSize = 3;

    // タイル管理用クラス
    class TileData
    {
        public Transform transform;
        public float[] positions;
        public float targetY;
        public Vector3 lastPos;
        public Vector3 velocity;
        public float timer;
    }

    private List<TileData> tiles = new List<TileData>();

    void Start()
    {
        Vector3 startPosition = transform.position;
        GenerateTiles(startPosition);
    }

    void GenerateTiles(Vector3 startPosition)
    {
        List<Vector2Int> selectedPositions = new List<Vector2Int>();
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        for (int blockX = 0; blockX < gridSize + plusColom; blockX += blockSize)
        {
            for (int blockZ = 0; blockZ < gridSize + plusRow; blockZ += blockSize)
            {
                for (int row = 0; row < blockSize; row++)
                {
                    int x = blockX + row;
                    int z = blockZ + Random.Range(0, blockSize);
                    selectedPositions.Add(new Vector2Int(x, z));
                }
            }
        }

        float[] positionsArray = new float[] { lowY, midY, highY };

        foreach (Vector2Int pos in selectedPositions)
        {
            float startY = positionsArray[Random.Range(0, positionsArray.Length)];
            Vector3 worldPos = new Vector3(
                startPosition.x + pos.x * (tileSize.x + spacing),
                startY,
                startPosition.z + pos.y * (tileSize.z + spacing)
            );

            GameObject tile = Instantiate(tilePrefab, worldPos, rot);

            TileData td = new TileData()
            {
                transform = tile.transform,
                positions = positionsArray,
                targetY = startY,
                lastPos = tile.transform.position,
                velocity = Vector3.zero,
                timer = Random.Range(0f, stayTime) // 個別遅延
            };
            tiles.Add(td);
        }

        Debug.Log($"Generated {selectedPositions.Count} tiles.");
    }

    void FixedUpdate()
    {
        foreach (var tile in tiles)
        {
            if (tile.timer > 0f)
            {
                tile.timer -= Time.fixedDeltaTime;
                continue;
            }

            // 上下移動 (Transformで直接動かす)
            float newY = Mathf.MoveTowards(tile.transform.position.y, tile.targetY, moveSpeed * Time.fixedDeltaTime);
            Vector3 nextPos = new Vector3(tile.transform.position.x, newY, tile.transform.position.z);
            tile.transform.position = nextPos;

            // 速度計算
            tile.velocity = (nextPos - tile.lastPos) / Time.fixedDeltaTime;
            tile.lastPos = nextPos;

            // 目標到達で次ターゲットをランダム設定
            if (Mathf.Approximately(newY, tile.targetY))
            {
                tile.targetY = tile.positions[Random.Range(0, tile.positions.Length)];
                tile.timer = stayTime;
            }
        }
    }
}
