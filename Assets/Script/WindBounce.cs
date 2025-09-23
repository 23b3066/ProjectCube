using UnityEngine;

public class WindBounce : MonoBehaviour
{
    [Header("跳ね返す対象のオブジェクト")]
    [Tooltip("パーティクルが衝突した際に跳ね返す対象のオブジェクトをInspectorで設定してください")]
    public GameObject targetObject;
    public GameObject moveObject;

    [Header("跳ね返しの強さ")]
    [Tooltip("衝突時に加える反発力（インパルス）")]
    public float bounceForce = 3f;

    // パーティクルが衝突したときに呼ばれるコールバック
    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other + "に当たった");
        // 対象オブジェクトが未設定の場合は処理を中断
        if(targetObject == null)
        {
            Debug.LogWarning("targetObjectが設定されていません。Inspectorで対象オブジェクトを設定してください。");
            return;
        }

        // 当たったオブジェクトが指定した対象オブジェクトの場合のみ処理
        if(other == targetObject)
        {
            Debug.Log("プレイヤーに当たりました。");

            // 対象オブジェクトのRigidbodyを取得
            Rigidbody rb = moveObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                // パーティクルシステムの位置から対象オブジェクトまでの方向を算出して正規化
                Vector3 bounceDirection = new Vector3(-1 * bounceForce,0.0f,0.0f);
                // インパルスモードで反発力を加える
                rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
            }
        }
    }
}
