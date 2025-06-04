using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("총알 프리팹")]
    [SerializeField] private BulletSpawn bulletPrefab;

    [Header("총알 발사 위치")]
    [SerializeField] private Transform firePoint;

    [Header("발사 쿨타임")]
    [SerializeField] private float fireRate = 0.08f;
    private float lastFireTime;

    [Header("탄약 및 리로딩 설정")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float reloadTime = 3f;
    private int currentAmmo;
    private bool isReloading = false;

    private PlayerMovement playerMovement;
    private Camera aimCamera;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        aimCamera = playerMovement.aimCamera;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (playerMovement == null || !playerMovement.aimCamera.gameObject.activeSelf) return;

        // 리로딩 중엔 발사 금지
        if (isReloading) return;

        // 우클릭 중 좌클릭 시 발사
        if (Input.GetMouseButton(1) && Input.GetMouseButton(0))
        {
            if (currentAmmo > 0 && Time.time - lastFireTime >= fireRate)
            {
                Fire();
                lastFireTime = Time.time;
            }
            else if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    void Fire()
    {
        // 카메라 중심 방향으로 발사
        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 fireDirection = ray.direction;

        BulletSpawn bullet = BulletSpawn.Spawn(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(fireDirection)
        );
        bullet.BulletStartDirection(fireDirection);

        currentAmmo--;
        Debug.Log($"발사! 남은 탄약: {currentAmmo}");
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("리로딩 중...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("리로딩 완료!");
    }
}
