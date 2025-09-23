using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TimerTextMeshPro : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 45f;           // タイマー開始値
    public TMP_Text timerText;                  // TMPテキスト

    [Header("End UI Settings")]
    public GameObject endUIPanel;               // ゲームオーバーUI
    public float returnDelay = 3f;              // シーン切り替えまでの遅延
    public float fadeDuration = 1f;             // UIフェード時間

    [Header("Player Respawn Settings")]
    public Transform teleportDestination;       // リスポーン位置
    public List<GameObject> players;            // リスポーンするプレイヤーリスト

    [Header("Gimmick Controller")]
    public GimmickController_test gimmickController; // FloorCount参照用

    private bool isTimerRunning = true;
    private CanvasGroup endUIGroup;

    void Start()
    {
        if (timerText == null)
            Debug.LogError("TimerTextMeshPro: TMP_Textが未設定です。");

        if (endUIPanel != null)
        {
            endUIGroup = endUIPanel.GetComponent<CanvasGroup>();
            if (endUIGroup == null)
                endUIGroup = endUIPanel.AddComponent<CanvasGroup>();
            endUIGroup.alpha = 0f;
            endUIPanel.SetActive(false);
        }

        if (gimmickController == null)
            gimmickController = FindObjectOfType<GimmickController_test>();
    }

    void Update()
    {
        if (!isTimerRunning) return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            timeRemaining = 0f;
            DisplayTime(0f);
            TimerEnd();
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timerText.text = Mathf.Max(timeToDisplay, 0f).ToString("F1");
    }

    void TimerEnd()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void RespawnPlayer(GameObject player)
    {
        if (player == null || teleportDestination == null) return;

        player.transform.position = teleportDestination.position;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log($"{player.name} has respawned at {player.transform.position}");
    }
}
