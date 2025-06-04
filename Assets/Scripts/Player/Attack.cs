using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("�Ѿ� ������")]
    [SerializeField] private BulletSpawn bulletPrefab;

    [Header("�Ѿ� �߻� ��ġ")]
    [SerializeField] private Transform firePoint;

    [Header("�߻� ��Ÿ��")]
    [SerializeField] private float fireRate = 0.08f;
    private float lastFireTime;

    [Header("ź�� �� ���ε� ����")]
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

        // ���ε� �߿� �߻� ����
        if (isReloading) return;

        // ��Ŭ�� �� ��Ŭ�� �� �߻�
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
        // ī�޶� �߽� �������� �߻�
        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 fireDirection = ray.direction;

        BulletSpawn bullet = BulletSpawn.Spawn(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(fireDirection)
        );
        bullet.BulletStartDirection(fireDirection);

        currentAmmo--;
        Debug.Log($"�߻�! ���� ź��: {currentAmmo}");
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("���ε� ��...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("���ε� �Ϸ�!");
    }
}
