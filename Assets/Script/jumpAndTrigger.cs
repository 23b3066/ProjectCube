using UnityEngine;
using Oculus.Interaction.Locomotion;

public class jumpAndTrigger : MonoBehaviour
{
    [SerializeField] private FirstPersonLocomotor locomotor;

    void Update()
    {
        // 右コントローラー
        bool aPressed = OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);
        bool triggerPressed = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        // Aボタン単独押下のみジャンプ
        if (aPressed && !triggerPressed)
        {
            locomotor.Jump();
        }
    }
}
