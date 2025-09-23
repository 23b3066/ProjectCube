using UnityEngine;
using System.Collections;

public class TileTransform : MonoBehaviour
{
    // 高さの3段階
    public float lowY = 0f;
    public float midY = 2f;
    public float highY = 4f;

    // 移動速度
    public float moveSpeed = 2f;

    // 到達したときに滞在する時間
    public float stayTime = 1.5f;

    // 内部用
    private float[] positions;
    private float targetY;
    private bool isMoving = false;

    void Start()
    {
        // 高さリストを配列に入れる
        positions = new float[] { lowY, midY, highY };

        // 最初の目標を設定
        PickNewTarget();
    }

    void Update()
    {
        if (isMoving)
        {
            // 現在位置
            Vector3 pos = transform.position;

            // 移動（Y座標だけ変更）
            float newY = Mathf.MoveTowards(pos.y, targetY, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(pos.x, newY, pos.z);

            // 到達判定
            if (Mathf.Approximately(newY, targetY))
            {
                isMoving = false;
                StartCoroutine(StayAndMoveAgain());
            }
        }
    }

    IEnumerator StayAndMoveAgain()
    {
        // 一定時間待つ
        yield return new WaitForSeconds(stayTime);

        // 次の目標を選択
        PickNewTarget();
    }

    void PickNewTarget()
    {
        // ランダムでターゲット選択
        float newTarget = targetY;
        while (newTarget == targetY)
        {
            newTarget = positions[Random.Range(0, positions.Length)];
        }
        targetY = newTarget;
        isMoving = true;
    }
}
