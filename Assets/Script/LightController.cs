using UnityEngine;

public class LightController : MonoBehaviour
{
    public SerialHandler serialHandler; // シリアル通信ハンドラー（Inspectorで割り当て）
    public Light[] lights;              // 制御対象のライト（9個以上、Inspectorで割り当て）

    private bool[] buttonDisabled;      // 各ライト（グループ単位で管理）の無効化状態
    private float[] disableTimestamps;  // 各ライトの無効化開始時刻
    private float disableDuration = 2f; // 2秒間無効化

    void Start()
    {
        // ライト配列のサイズに合わせて初期化
        buttonDisabled = new bool[lights.Length];
        disableTimestamps = new float[lights.Length];

        serialHandler.OnDataReceived += OnDataReceived; // シリアル受信イベント登録

        // すべてのライトを初期状態（点灯）に設定
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = true;
            buttonDisabled[i] = false;
        }
    }

    void OnDestroy()
    {
        serialHandler.OnDataReceived -= OnDataReceived; // イベント解除
    }

    void Update()
    {
        float currentTime = Time.time;
        // 各ライトの無効化状態をチェックし、2秒経過していれば再点灯
        for (int i = 0; i < lights.Length; i++)
        {
            if (buttonDisabled[i] && currentTime - disableTimestamps[i] >= disableDuration)
            {
                buttonDisabled[i] = false; // 無効化解除
                lights[i].enabled = true;  // ライト再点灯
            }
        }
    }

    void OnDataReceived(string message)
    {
        Debug.Log($"Received: {message}");

        // 受信メッセージは "fX" 形式（例："f6"）と仮定
        if (message.Length >= 2 && message[0] == 'f')
        {
            if (int.TryParse(message.Substring(1), out int buttonIndex))
            {
                // ここで「グループの先頭」だけを処理するため、buttonIndexが 0, 3, 6 のときのみ実行
                if (buttonIndex % 3 == 0)
                {
                    int group = buttonIndex / 3; // グループ番号（0, 1, 2）
                    int startIdx = group * 3;    // グループ内の先頭インデックス

                    // グループ内の全ライト（startIdx, startIdx+1, startIdx+2）をまとめて操作
                    for (int i = 0; i < 3; i++)
                    {
                        int idx = startIdx + i;
                        if (idx < lights.Length && !buttonDisabled[idx])
                        {
                            lights[idx].enabled = false; // ライトを消灯
                            buttonDisabled[idx] = true;  // 無効化
                            disableTimestamps[idx] = Time.time; // 無効化開始時刻を記録
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning($"無効なボタンインデックス: {message.Substring(1)}");
            }
        }
        else
        {
            Debug.LogWarning($"受信メッセージの形式が不正: {message}");
        }
    }
}

