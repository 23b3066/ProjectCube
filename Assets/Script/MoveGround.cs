using UnityEngine;

public class UpDownMovement : MonoBehaviour
{
    // 上下運動の幅
    public float amplitude = 1f;
    // 上下運動の速度
    public float speed = 1f;
    // 元の位置を保持
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // 時間に応じて上下運動
        float y = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0, y, 0);
    }
}
