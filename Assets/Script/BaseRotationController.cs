using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class BaseRotationController : MonoBehaviour
{
    public Animator animator;
    public Transform cube;               // 持ち上げたい対象
    public float acceleration = 1.5f;
    public float deceleration = 1.5f;
    public float maxSpeed = 3f;
    public float liftSpeed = 0.05f;
    public float liftAmount = 0.05f;

    private float currentSpeed = 0f;
    private float currentLift = 0f;
    private Vector3 baseCubePosition;

    private bool isHovering = false;

    void Start()
    {
        if (cube != null)
        {
            baseCubePosition = cube.position;
        }
    }

    void Update()
    {
        // 回転速度更新
        if (isHovering && currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deceleration * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        animator.speed = currentSpeed;

        // 持ち上げ処理
        if (cube != null)
        {
            if (isHovering)
            {
                currentLift = Mathf.Min(currentLift + liftSpeed * Time.deltaTime, liftAmount);
            }
            else
            {
                currentLift = Mathf.Max(currentLift - liftSpeed * Time.deltaTime, 0f);
            }

            cube.position = baseCubePosition + new Vector3(0f, currentLift, 0f);
        }
    }

    public void OnHoverEnter()
    {
        isHovering = true;
        Debug.Log("hover");
    }

    public void OnHoverExit()
    {
        isHovering = false;
        Debug.Log("unhover");
    }
}