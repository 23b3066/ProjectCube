using UnityEngine;
using TMPro; // TextMeshPro を使う場合

public class RadialMenuManager: MonoBehaviour
{
    [Header("メニューアイコン（円周上に配置）")]
    public RectTransform[] menuIcons;

    [Header("ハイライトカーソル")]
    public RectTransform highlightCursor;

    [Header("中央のテキスト (親ごと)")]
    public GameObject centerText;

    [Header("メニュー全体")]
    public GameObject radialMenuRoot;

    [Header("入力感度")]
    public float deadZone = 0.5f;

    [Header("ハイライトの角度ごとの位置調整")]
    public Vector2[] highlightOffsets;  // セグメント数と同じ長さで設定

    [Header("中央に表示するテキスト")]
    public TextMeshProUGUI centerLabelText; // TextMeshProUGUI をアタッチ

    [Tooltip("各セグメントに対応する表示文字列")]
    public string[] segmentLabels; // menuIconsの数と同じ長さにする

    [Header("ポインター・手オブジェクト")]
    public GameObject pointer;
    public GameObject hand;

    private int currentIndex = -1;
    private bool menuVisible = false;

    public WallDragPlacer WallDragPlacer;
    

    void Start()
    {
        SetMenuVisible(false); // 最初は非表示
    }

    void Update()
    {
        HandleBButton();
        if (menuVisible)
        {
            HandleStickSelection();
        }
    }

    void HandleBButton()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two)) // Bボタン
        {
            menuVisible = !menuVisible;
            SetMenuVisible(menuVisible);
        }
    }

   void HandleStickSelection()
{
    Vector2 axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

    if (axis.magnitude < deadZone)
    {
        currentIndex = -1;
        highlightCursor.gameObject.SetActive(false);
        return;
    }

    float angle = Mathf.Atan2(axis.x, axis.y) * Mathf.Rad2Deg;
    if (angle < 0) angle += 360f;


    //angle = 360f - angle;  // ← 左右反転をここでやる

    int segmentCount = menuIcons.Length;  // 3に固定しないでmenuIconsに合わせるのがおすすめ
    int segmentAngle = 360 / segmentCount;

    int index = Mathf.FloorToInt((angle + segmentAngle / 2f) / segmentAngle) % segmentCount;

    if (index != currentIndex)
    {
        currentIndex = index;
        UpdateHighlight();
    }

    highlightCursor.gameObject.SetActive(true);
}

    void UpdateHighlight()
{
    if (currentIndex >= 0 && currentIndex < menuIcons.Length)
    {
        Vector2 offset = Vector2.zero;

        if (highlightOffsets != null && highlightOffsets.Length == menuIcons.Length)
        {
            offset = highlightOffsets[currentIndex];
        }

        Vector2 correctedOffset = new Vector2(-offset.x, offset.y);
        highlightCursor.localPosition = menuIcons[currentIndex].localPosition + (Vector3)correctedOffset;

        int segmentAngle = 360 / menuIcons.Length;
        float highlightAngle = currentIndex * segmentAngle;
        highlightCursor.localEulerAngles = new Vector3(0f, 0f, 360f - highlightAngle);

        // 中央のテキストを更新
        if (centerLabelText != null && segmentLabels != null && segmentLabels.Length > currentIndex)
        {
            centerLabelText.text = segmentLabels[currentIndex];
        }

        if (WallDragPlacer.Instance != null)
        {
            WallDragPlacer.Instance.SetSelectedWallIndex(currentIndex);
        }


        highlightCursor.gameObject.SetActive(true);
    }
}


    void SetMenuVisible(bool visible)
    {
        radialMenuRoot.SetActive(visible);
        highlightCursor?.gameObject.SetActive(visible);
        centerText?.SetActive(visible);

        foreach (var icon in menuIcons)
        {
            if (icon != null) icon.gameObject.SetActive(visible);
        }

        // パイメニューが開いているかを WallDragPlacer に通知
        if (WallDragPlacer != null)
        {
            WallDragPlacer.IsPlacementEnabled = visible;
        }

        // ポインターと手も切り替え
        if (pointer != null) pointer.SetActive(visible);
        if (hand != null) hand.SetActive(visible);
    }
}

    
