using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ComplementColliderCapsule : MonoBehaviour
{
    public Transform TipPosition;

    // Collider 参照
    private CapsuleCollider capsuleCollider;

    // 前フレーム位置
    private Vector3 lastTipPosition;
    private Vector3 lastPosition;

    void Start()
    {
        // Rigidbody 設定
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        // CapsuleCollider がなければ追加
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider == null)
            capsuleCollider = gameObject.AddComponent<CapsuleCollider>();

        capsuleCollider.isTrigger = true;

        // 前回位置の初期化
        lastTipPosition = TipPosition.position;
        lastPosition = transform.position;

        // 初期カプセル更新
        UpdateCapsule();
    }

    void FixedUpdate()
    {
        // 動いていなければ処理しない
        if (lastPosition == transform.position && lastTipPosition == TipPosition.position)
            return;

        UpdateCapsule();

        lastPosition = transform.position;
        lastTipPosition = TipPosition.position;
    }

    void UpdateCapsule()
    {
        Vector3 start = transform.position;
        Vector3 end = TipPosition.position;

        // カプセルの中心は start と end の中点
        capsuleCollider.center = transform.InverseTransformPoint((start + end) * 0.5f);

        // カプセルの高さは start と end の距離 + 半径分
        float distance = Vector3.Distance(start, end);
        capsuleCollider.height = distance;

        // カプセルの方向（Y軸方向で回転）
        Vector3 direction = (end - start).normalized;

        // CapsuleCollider の向きを Y軸に合わせる
        // Unity の CapsuleCollider では 0=X,1=Y,2=Z
        capsuleCollider.direction = 1; // Y軸方向

        // 半径は距離に応じて調整
        capsuleCollider.radius = 0.05f; // 必要に応じて調整
    }
}
