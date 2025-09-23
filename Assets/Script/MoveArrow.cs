using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    public float speed = 1f;      // オブジェクトの移動速度（1秒あたりの移動距離）
    public float startPos = 62f;  // 移動範囲の始点（x座標）
    public float endPos = 53f;    // 移動範囲の終点（x座標）

    private bool movingRight = true; // 現在右方向に移動しているかどうかのフラグ
    private Vector3 pos;             // 現在の座標を保持する変数

    void Start()
    {
        // 現在の位置を取得
        pos = transform.position;

        // 始点と終点を整理（startPos < endPos になるようにする）
        if (startPos > endPos)
        {
            float temp = startPos;
            startPos = endPos;
            endPos = temp;
        }

        // 現在位置を startPos〜endPos の範囲に強制補正（はみ出さないように）
        pos.x = Mathf.Clamp(pos.x, startPos, endPos);
        transform.position = pos;
    }

    void Update()
    {
        // 毎フレーム位置を取得
        pos = transform.position;

        if (movingRight)
        {
            // 右方向（x座標を増加）に移動
            pos.x += speed * Time.deltaTime;

            // 終点を超えたら位置を補正して反転
            if (pos.x >= endPos)
            {
                pos.x = endPos;
                movingRight = false; // 左方向へ切り替え
            }
        }
        else
        {
            // 左方向（x座標を減少）に移動
            pos.x -= speed * Time.deltaTime;

            // 始点を下回ったら位置を補正して反転
            if (pos.x <= startPos)
            {
                pos.x = startPos;
                movingRight = true; // 右方向へ切り替え
            }
        }

        // 実際に位置を更新
        transform.position = pos;
    }
}
