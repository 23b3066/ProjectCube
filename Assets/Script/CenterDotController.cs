using UnityEngine;

public class CenterDotController : MonoBehaviour
{
    public Camera mainCamera;  // メインカメラ
    public RectTransform dot; // 点のRectTransform

    // Dotの適切な距離設定（カメラから少し前に配置する）
    public float distanceFromCamera = 1.0f; // カメラからの距離

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // カメラを自動で取得
        }

        if (dot == null)
        {
            Debug.LogError("CenterDotController: Dotが設定されていません。");
        }
    }

    void Update()
    {
        // カメラの中心に点を配置するため、適切な位置を設定
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, distanceFromCamera); // 画面の中央の位置（カメラからの距離を指定）

        // ViewportからWorld座標に変換
        dot.position = mainCamera.ViewportToWorldPoint(screenCenter); 
    }
}
