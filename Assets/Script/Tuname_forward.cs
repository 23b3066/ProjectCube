using UnityEngine;

public class Tuname_forward : MonoBehaviour
{
    public float speed = 5f;

    private Vector3 initialLocalPosition;

    void Awake()
    {
        // 初期ローカル位置を保存（Awakeはオブジェクト生成時に一度だけ呼ばれる）
        initialLocalPosition = transform.localPosition;
    }

    void OnEnable()
    {
        // オブジェクトが有効になったときにローカル位置を初期化
        transform.localPosition = initialLocalPosition;
    }

    void Update()
    {
        // 前方に移動（ローカル座標のZ軸方向）
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
