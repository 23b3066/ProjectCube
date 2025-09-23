using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject controlsPanel; // 操作説明UI

    void Start()
    {
        if (controlsPanel != null)
            controlsPanel.SetActive(false); // 最初は非表示
    }

    void Update()
    {
        // 左手のXボタンが押されたら
        if (OVRInput.GetDown(OVRInput.Button.Three)) // Xボタン
        {
            if (controlsPanel != null)
                controlsPanel.SetActive(!controlsPanel.activeSelf); // 表示/非表示切替
        }
    }
}
