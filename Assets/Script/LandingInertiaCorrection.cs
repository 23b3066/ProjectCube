using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LandingInertiaCorrection : MonoBehaviour
{
    [Header("Settings")]
    public Transform groundCheck;          // 足元判定用
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Horizontal Movement Speed")]
    public float groundMoveSpeed = 3f;     // 着地時に適用する速度

    private Rigidbody rb;
    private bool wasGroundedLastFrame = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 地上判定
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // 空中から地上に着地した瞬間
        if (!wasGroundedLastFrame && isGrounded)
        {
            CorrectLandingInertia();
        }

        wasGroundedLastFrame = isGrounded;
    }

    private void CorrectLandingInertia()
    {
        Vector3 vel = rb.linearVelocity;

        // 着地した瞬間の空中慣性を打ち消す
        // 横方向の速度だけ置き換える
        Vector3 horizontalVel = new Vector3(vel.x, 0, vel.z);
        if (horizontalVel.magnitude > groundMoveSpeed)
        {
            horizontalVel = horizontalVel.normalized * groundMoveSpeed;
        }

        rb.linearVelocity = new Vector3(horizontalVel.x, vel.y, horizontalVel.z);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
