using UnityEngine;

public class HeightAreaChecker : MonoBehaviour
{
    public Transform player;       // プレイヤーのTransform
    public GameObject[] targetObjs; // 有効化/無効化するオブジェクト（3つ想定）

    // 高さのしきい値
    public float lowHeight = 3.0f;
    public float midHeight = 6.0f;

    void Update()
    {
        if (player == null || targetObjs.Length < 3) return;

        // すべて無効化
        foreach (GameObject obj in targetObjs)
        {
            obj.SetActive(false);
        }

        float y = player.position.y;

        // 下のエリア
        if (y < lowHeight)
        {
            targetObjs[0].SetActive(true);
        }
        // 中間のエリア
        else if (y < midHeight)
        {
            targetObjs[1].SetActive(true);
        }
        // 上のエリア
        else
        {
            targetObjs[2].SetActive(true);
        }
    }
}
