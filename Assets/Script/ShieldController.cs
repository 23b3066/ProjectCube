using UnityEngine;
using UnityEngine.VFX; // VFX 用
using Oculus.Interaction.Locomotion;

public class ShieldController : MonoBehaviour
{
    [Header("対象の VFX Graph")]
    public GameObject vfxObject;

    [Header("有効になるX回転角度の範囲")]
    public float minX = 30f;
    public float maxX = 60f;

    [Header("有効になるY回転角度の範囲")]
    public float minY = 30f;
    public float maxY = 60f;

    [Header("関連コンポーネント")]
    public WeaponSwitcher weaponSwitcher;
    
    public FirstPersonLocomotor locomotor; // Inspectorで設定

    private float DefoultSpeedFactor;

    

    private bool isActive = false;

    void Start()
    {
        DefoultSpeedFactor = locomotor.SpeedFactor;
        if (vfxObject != null)
            vfxObject.SetActive(false);

    }

    void Update()
    {
        if (vfxObject == null || weaponSwitcher == null)
            return;

        // 現在のローカル回転角度を取得
        float xRotation = transform.localEulerAngles.x;
        float yRotation = transform.localEulerAngles.y;

        // Unity の角度は 0~360 なので補正
        if (xRotation > 180f) xRotation -= 360f;
        if (yRotation > 180f) yRotation -= 360f;

        // シールド有効条件
        bool inAngleRange = xRotation >= minX && xRotation <= maxX &&
                            yRotation >= minY && yRotation <= maxY;

        // 武器が非アクティブかつ角度範囲内ならシールド有効
        if (!weaponSwitcher.IsGunActive && inAngleRange)
        {
            if (!isActive)
            {
                ActivateShield();
            }
        }
        else
        {
            if (isActive)
            {
                DeactivateShield();
            }
        }
    }

    private void ActivateShield()
    {
        if (vfxObject != null)
            vfxObject.SetActive(true);

        isActive = true;
        locomotor.SpeedFactor = 0f;
        
    }

    private void DeactivateShield()
    {
        if (vfxObject != null)
            vfxObject.SetActive(false);

        isActive = false;
       locomotor.SpeedFactor = DefoultSpeedFactor;
        
    }
}
