using UnityEngine;

public class LookAtTargetSlerp : MonoBehaviour
{
    [SerializeField] private Transform target; // 追従したいオブジェクト
    [SerializeField] private float rotationSpeed = 3f; // 回転速度

    void Update()
    {
        if (target == null) return;

        // ターゲットの方向ベクトル
        Vector3 direction = (target.position - transform.position).normalized;

        // 向きたい回転
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Y軸に-90°補正を加える
        targetRotation *= Quaternion.Euler(0, -90f, 0);

        // Slerpで現在の回転から目標回転へ補間
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }
}
