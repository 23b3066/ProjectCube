using UnityEngine;
using System.Collections;

public class EnemyChase : MonoBehaviour
{
    [Header("プレイヤー情報")]
    public Transform player;

    [Header("移動パラメータ")]
    public float speed = 3f;
    public float rotationSpeed = 5f;

    [Header("攻撃パラメータ")]
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public float knockBackPower = 5f;
    public float knockBackDelay = 0.5f;
    private float lastAttackTime;

    [Header("アニメーション・エフェクト")]
    public Animator animator;
    public GameObject shield;
    public GameObject AttackEffect;
    public AudioSource attackSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (AttackEffect != null)
            AttackEffect.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        // プレイヤーの方向を常に向く
        LookAtPlayer();

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // 攻撃範囲外なら移動
            MoveTowardsPlayer();
            animator.SetBool("isMoving", true);
        }
        else
        {
            // 攻撃範囲内なら攻撃
            animator.SetBool("isMoving", false);
            if (Time.time - lastAttackTime > attackCooldown)
            {
                StartCoroutine(AttackPlayerWithDelay());
                lastAttackTime = Time.time;
            }
        }
    }

    // プレイヤーの方向を常に向く処理
    // プレイヤーの方向を常に向く
    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction.magnitude > 0f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // 前方に移動
    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }


    IEnumerator AttackPlayerWithDelay()
    {
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(knockBackDelay);

        // ノックバック処理
        // knockBack();
    }

    public void knockBack()
    {
        if (player == null) return;

        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        if (playerRb != null && (shield == null || !shield.activeSelf) && AttackEffect.activeSelf)
        {
            // ノックバック方向を計算
            Vector3 knockBackDir = (player.position - transform.position).normalized;
            knockBackDir.y = 0f;
            knockBackDir.Normalize();

            // プレイヤーをノックバック
            playerRb.linearVelocity = Vector3.zero;
            playerRb.AddForce(knockBackDir * knockBackPower, ForceMode.VelocityChange);
        }
    }

    public void ShowAttackEffect()
    {

        AttackEffect.SetActive(true);
        attackSound.Play();
    }

    public void HideAttackEffect()
    {

        AttackEffect.SetActive(false);
        attackSound.Stop();
    }
    
    public IEnumerator Explosion()
    {   
        yield return new WaitForSeconds(3f);
        AttackEffect.SetActive(true);
        attackSound.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
