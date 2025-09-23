using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformWarning : MonoBehaviour
{    
    public RectTransform warning;
    public float lastPosition;
    public float startPosition;
    Vector2 pos;

    public float speed = 100f; // 移動速度（px/sec）

    void Start()
    {
        pos = warning.anchoredPosition;
    }

    void Update()
    {
        pos = warning.anchoredPosition; // 毎フレーム最新位置を取得

        if (pos.x <= lastPosition)
        {
            pos.x = startPosition;
        }
        pos.x -= speed * Time.deltaTime; // 時間に依存した移動
        warning.anchoredPosition = pos;  // anchoredPosition を更新
    }
}
