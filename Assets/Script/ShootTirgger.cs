using UnityEngine;

public class ShootTrigger : MonoBehaviour
{
    // Animatorコンポーネントをアサイン
    public Animator animator;

    // Animatorで作ったTriggerの名前
    public string triggerName = "TriggerOn";

    void Update()
    {
        // スペースキーが押されたらTrigger発火
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            animator.SetTrigger(triggerName);
        }
    }
}
