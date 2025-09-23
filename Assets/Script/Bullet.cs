using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 50f;               // 弾の速度
    public float lifetime = 5f;             // 自動消滅までの時間
    public string wallTag = "Wall";         // 壁タグ
    public string enemyTag = "Enemy";       // 敵タグ
    public GameObject enemyDeathEffect;     // 敵死亡エフェクト
    public AudioSource DeathSound;         // 弾発射音

    private void Start()
    {
        Destroy(gameObject, lifetime); // 一定時間で消滅
    }

    private void Update()
    {
        // 前方に移動
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

   private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(wallTag) || collision.gameObject.CompareTag("MovingPlatform"))
        {
            // 壁に当たったら弾だけ消す
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag(enemyTag))
        {

            Destroy(gameObject);           // 弾を消す
            Destroy(collision.gameObject); // 敵を消す
            // 敵に当たったらエフェクトを生成して敵も弾も消す
            Instantiate(enemyDeathEffect, collision.transform.position, Quaternion.identity);
            DeathSound.Play();
        }
    }


}
