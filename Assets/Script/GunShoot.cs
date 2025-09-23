using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject muzzleFlashPrefab;
    [SerializeField] LineRenderer lineRenderer; // レーザー可視化用

    [Header("Gun Properties")]
    [SerializeField] float timeBetweenShooting = 0.2f;
    [SerializeField] float rayDistance = 100f;

    private float shootCooldown = 0f;

    public AudioSource shootSound;

    void Update()
    {
        // レーザー表示
        //UpdateLaser();

        // クールダウン管理
        if (shootCooldown > 0f)
            shootCooldown -= Time.deltaTime;

        // 右手トリガーで弾を発射
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && shootCooldown <= 0f)
        {
            
            Shoot();
            shootSound.Play();
            shootCooldown = timeBetweenShooting;
        }
    }

    void Shoot()
    {
        // Z軸方向に射線
        Vector3 rayDirection = shootPoint.forward;

        // 弾生成
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.LookRotation(rayDirection));

        // マズルフラッシュ
        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, shootPoint.position, Quaternion.LookRotation(rayDirection));
            flash.transform.SetParent(shootPoint);
            Destroy(flash, 0.2f);
        }
    }

    void UpdateLaser()
    {
        if (lineRenderer == null) return;

        // Z軸方向に射線を飛ばす
        Vector3 rayDirection = shootPoint.forward;

        Ray ray = new Ray(shootPoint.position, rayDirection);
        Vector3 endPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            endPoint = hit.point;
        else
            endPoint = shootPoint.position + rayDirection * rayDistance;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, endPoint);
    }

    void LateUpdate()
    {
        // レーザー表示を更新
        UpdateLaser();
    }

}
