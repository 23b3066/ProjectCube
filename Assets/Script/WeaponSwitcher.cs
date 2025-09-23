using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject gun;   // 銃のオブジェクト
    public GameObject sword; // 剣のオブジェクト

    private bool isGunActive = true;

    public bool IsGunActive => isGunActive;

    private void Start()
    {
        // 初期状態を明示的に設定
        SetWeapon(true);
    }

    private void Update()
    {
        // Bボタンを押したら切り替え
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            ToggleWeapon();
        }
    }

    private void ToggleWeapon()
    {
        isGunActive = !isGunActive;
        SetWeapon(isGunActive);
    }

    private void SetWeapon(bool gunActive)
    {
        if (gun != null) gun.SetActive(gunActive);
        if (sword != null) sword.SetActive(!gunActive);
    }
}
