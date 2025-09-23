using UnityEngine;

public class OVRMovingPlatformFollower : MonoBehaviour
{
    private Transform platform;        // 今乗ってる床
    private Vector3 lastPlatformPos;   // 前フレームの床の位置

    public Transform playerPosition;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            Debug.Log("MovingPlatformに接触");
            platform = other.transform;
            lastPlatformPos = platform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            Debug.Log("MovingPlatformから離れた");
            platform = null;
        }
    }

    void LateUpdate()
    {
        if (platform != null)
        {
            // 床の移動分を計算してプレイヤーに加算
            Vector3 delta = platform.position - lastPlatformPos;
            playerPosition.position += delta;
            lastPlatformPos = platform.position;
        }
    }
}
