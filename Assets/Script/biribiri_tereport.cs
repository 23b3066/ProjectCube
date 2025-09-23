using UnityEngine;

public class biribiri_tereport : MonoBehaviour
{
    public string resporn_point = "resporn_point03"; // テレポート先のオブジェクト名
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("ok");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Playerに当たった");
            GameObject target = GameObject.Find(resporn_point); // 移動先のオブジェクト名
            GameObject cameraRig = GameObject.Find("OVRCameraRig");
            GameObject playerController = GameObject.Find("PlayerController");
            var rb_cameraRig = cameraRig.GetComponent<Rigidbody>();
            var rb_playerController = playerController.GetComponent<Rigidbody>();
            if (target != null)
            {
                cameraRig.transform.position = target.transform.position;
                playerController.transform.position = target.transform.position;
                rb_cameraRig.linearVelocity = Vector3.zero;
                rb_cameraRig.angularVelocity = Vector3.zero;
                rb_playerController.linearVelocity = Vector3.zero;
                rb_playerController.angularVelocity = Vector3.zero;
            }
            else
            {
                Debug.LogWarning("WarpPoint が見つかりません");
            }

            // 例：このオブジェクトを消す場合
            Destroy(gameObject);
        }
    }
}

