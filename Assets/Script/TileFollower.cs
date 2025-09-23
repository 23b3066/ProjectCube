using UnityEngine;

public class TileFollower : MonoBehaviour
{
    public Rigidbody playerRb;

    void Start()
    {
        if (playerRb == null)
        {
            GameObject player = GameObject.FindWithTag("Player"); // プレイヤーに"Player"タグが必要
            if (player != null)
                playerRb = player.GetComponent<Rigidbody>();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerRb != null)
        {
            Vector3 moveDelta = transform.position - transform.position; // タイルの移動量を計算
            playerRb.MovePosition(playerRb.position + moveDelta);
        }
    }
}
