using UnityEngine;

public class BuildingWall : MonoBehaviour
{
    public GameObject temWall;
    public GameObject wall;

    // Grid上の位置（x = X軸, y = Z軸）
    public Vector2Int gridPos;

    // 横方向（行）なら true、縦方向（列）なら false
    public bool IsRow;

    public bool IsTempPlaced { get; private set; } = false;
    public bool IsPlaced { get; private set; } = false;

    private GameObject[] wallPrefabs; // インスペクターから3つの壁を設定
    private int selectedWallIndex = 0; // ラジアルメニューから更新される番号

    private void Awake()
    {
        // Z軸を y にマッピングして gridPos を決定
        gridPos = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );
    }

    private void Start()
    {
        WallDragPlacer.Instance.Register(this);
        ResetAll();
    }

    public void ShowTemp()
    {
        if (!IsPlaced)
        {
            temWall.SetActive(true);
            IsTempPlaced = true;
        }
    }

    public void HideTemp()
    {
        if (!IsPlaced)
        {
            temWall.SetActive(false);
            IsTempPlaced = false;
        }
    }

    public void Confirm(GameObject wallPrefab)
    {
        if (!IsPlaced)
        {
            // 仮の壁を非表示
            temWall.SetActive(false);

            // すでに設置されている wall オブジェクトを非表示または削除
            if (wall != null)
            {
                wall.SetActive(false);
                //Destroy(wall); // 必要ならこちらを使う
            }

            // 新しいプレハブを生成
            if (wallPrefab != null)
            {
                GameObject newWall = Instantiate(wallPrefab, transform.position, transform.rotation, transform.parent);
                wall = newWall;
                wall.SetActive(true);
            }

            IsPlaced = true;
            IsTempPlaced = false;
        }
    }



    public void ResetAll()
    {
        wall.SetActive(false);
        temWall.SetActive(false);
        IsTempPlaced = false;
        IsPlaced = false;
    }

    public void OnSelected()
    {
        WallDragPlacer.Instance?.OnWallSelected(this);
    }

    public void OnHoverEnter()
    {
        WallDragPlacer.Instance?.OnWallHovered(this);
    }

    public void OnHoverExit()
    {
        WallDragPlacer.Instance?.OnWallUnhovered(this);
    }

    public void SetSelectedWallIndex(int index)
    {
    if (index >= 0 && index < wallPrefabs.Length)
        {
            selectedWallIndex = index;
        }
    }
}
